using AutoMapper;
using GreenSale.Application.Exceptions;
using GreenSale.Application.Exceptions.Auth;
using GreenSale.Application.Exceptions.Users;
using GreenSale.DataAccess.Interfaces.Roles;
using GreenSale.DataAccess.Interfaces.Users;
using GreenSale.DataAccess.ViewModels.UserRoles;
using GreenSale.Domain.Entites.Roles;
using GreenSale.Domain.Entites.Roles.UserRoles;
using GreenSale.Domain.Entites.Users;
using GreenSale.Persistence.Dtos;
using GreenSale.Persistence.Dtos.Auth;
using GreenSale.Persistence.Dtos.Notifications;
using GreenSale.Persistence.Dtos.Security;
using GreenSale.Service.Helpers;
using GreenSale.Service.Interfaces.Auth;
using GreenSale.Service.Interfaces.Notifications;
using GreenSale.Service.Security;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;

namespace GreenSale.Service.Service.Auth;

public class AuthServise : IAuthServices
{
    private const int CACHED_FOR_MINUTS_REGISTER = 60;
    private const int CACHED_FOR_MINUTS_VEFICATION = 5;
    private const int VERIFICATION_MAXIMUM_ATTEMPTS = 3;

    private const string REGISTER_CACHE_KEY = "register_";
    private const string VERIFY_REGISTER_CACHE_KEY = "verify_register_";
    private const string Reset_CACHE_KEY = "reset_";

    private readonly IUserRoles _userRoles;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenService _tokenService;
    private readonly ISmsSender _smsSender;
    private readonly IMemoryCache _memoryCache;
    private readonly IUserRepository _userRepository;

    public AuthServise(
        IMemoryCache memoryCache,
        IUserRepository userRepository,
        ITokenService tokenService,
        ISmsSender smsSender,
        IUserRoles userRoles,
        IConfiguration configuration)
    {
        this._userRoles = userRoles;
        this._tokenService = tokenService;
        this._smsSender = smsSender;
        this._memoryCache = memoryCache;
        this._userRepository = userRepository;
    }

    public async Task<(bool Result, int CachedMinutes)> RegisterAsync(UserRegisterDto dto)
    {
        var dbResult = await _userRepository.GetByPhoneAsync(dto.PhoneNumber);

        if (dbResult is not null)
            throw new UserAlreadyExistsException();

        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + dto.PhoneNumber, out UserRegisterDto registerDto))
        {
            registerDto.PhoneNumber = registerDto.PhoneNumber;
            _memoryCache.Remove(dto.PhoneNumber);
        }
        else
        {
            _memoryCache.Set(REGISTER_CACHE_KEY + dto.PhoneNumber, dto, TimeSpan.FromMinutes
                (CACHED_FOR_MINUTS_REGISTER));
        }

        return (Result: true, CachedMinutes: CACHED_FOR_MINUTS_REGISTER);
    }

    public async Task<(bool Result, int CachedVerificationMinutes)> SendCodeForRegisterAsync(string phoneNumber)
    {
        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + phoneNumber, out UserRegisterDto registerDto))
        {
            VerificationDto verificationDto = new VerificationDto();
            verificationDto.Attempt = 0;
            verificationDto.CreatedAt = TimeHelper.GetDateTime();
            verificationDto.Code = 12345;//CodeGenerator.CodeGeneratorPhoneNumber();
            _memoryCache.Set(phoneNumber, verificationDto, TimeSpan.FromMinutes(CACHED_FOR_MINUTS_VEFICATION));

            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + phoneNumber,
                out VerificationDto OldverificationDto))
            {
                _memoryCache.Remove(phoneNumber);
            }

            _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + phoneNumber, verificationDto,
                TimeSpan.FromMinutes(VERIFICATION_MAXIMUM_ATTEMPTS));

            SmsSenderDto smsSenderDto = new SmsSenderDto();
            smsSenderDto.Title = "Green sale\n";
            smsSenderDto.Content = "Your verification code : " + verificationDto.Code;
            smsSenderDto.Recipent = phoneNumber.Substring(1);
            var result = true;//await _smsSender.SendAsync(smsSenderDto);

            if (result is true)
                return (Result: true, CachedVerificationMinutes: CACHED_FOR_MINUTS_VEFICATION);
            else
                return (Result: false, CACHED_FOR_MINUTS_VEFICATION: 0);
        }
        else
        {
            throw new ExpiredException();
        }
    }

    public async Task<(bool Result, string Token)> VerifyRegisterAsync(string phoneNumber, int code)
    {
        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + phoneNumber, out UserRegisterDto userRegisterDto))
        {
            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + phoneNumber, out VerificationDto verificationDto))
            {
                if (verificationDto.Attempt >= VERIFICATION_MAXIMUM_ATTEMPTS)
                    throw new VerificationTooManyRequestsException();

                else if (verificationDto.Code == code)
                {
                    var dbresult = await RegisterToDatabaseAsync(userRegisterDto);
                    /*Role role = new Role()
                    {
                        Name = "User",
                        CreatedAt = TimeHelper.GetDateTime(),
                        UpdatedAt = TimeHelper.GetDateTime(),
                    };
                    var DbRoleResult = await _roleRepository.CreateAsync(role);*/
                    var DbRoleResult = 1;
                    if (dbresult != 0 && DbRoleResult != 0)
                    {
                        UserRole userRole = new UserRole()
                        {
                            UserId = dbresult,
                            RoleId = DbRoleResult,
                            CreatedAt = TimeHelper.GetDateTime(),
                            UpdatedAt = TimeHelper.GetDateTime(),
                        };
                        var DbUserRoles = await _userRoles.CreateAsync(userRole);
                        if (DbUserRoles > 0)
                        {
                            var user = await _userRepository.GetByIdAsync(dbresult);
                            UserRoleViewModel userRoleViewModel = new UserRoleViewModel()
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                PhoneNumber = user.PhoneNumber,
                                Region = user.Region,
                                District = user.District,
                                Address = user.Address,
                                RoleName = "User",
                                CreatedAt = TimeHelper.GetDateTime(),
                                UpdatedAt = TimeHelper.GetDateTime(),
                            };

                            string token = _tokenService.GenerateToken(userRoleViewModel);
                            return (Result: true, Token: token);
                        }
                        else
                            return (Result: false, Token: "");
                    }
                    else
                        return (Result: false, Token: "");
                }
                else
                {
                    _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + phoneNumber);
                    verificationDto.Attempt++;
                    _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + phoneNumber, verificationDto,
                        TimeSpan.FromMinutes(CACHED_FOR_MINUTS_VEFICATION));

                    return (Result: false, Token: "");
                }
            }
            else throw new VerificationCodeExpiredException();
        }
        else throw new ExpiredException();
    }

    public async Task<(bool Result, string Token)> LoginAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetByPhoneAsync(dto.PhoneNumber);
        if (user is null) throw new UserNotFoundException();

        var hasherResult = PasswordHasher.Verify(dto.password, user.Salt, user.PasswordHash);
        if (hasherResult == false) throw new PasswordNotMatchException();

        UserRoleViewModel userRoleViewModel = new UserRoleViewModel()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Region = user.Region,
            District = user.District,
            Address = user.Address,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };

        var Dbrole = await _userRoles.GetByIdAsync(user.Id);
        userRoleViewModel.RoleName = Dbrole.RoleName;

        string token = _tokenService.GenerateToken(userRoleViewModel);

        return (Result: true, Token: token);
    }

    private async Task<int> RegisterToDatabaseAsync(UserRegisterDto userRegisterDto)
    {
        User user = new User()
        {
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            PhoneNumber = userRegisterDto.PhoneNumber,
            Region = userRegisterDto.Region,
            District = userRegisterDto.District,
            Address = userRegisterDto.Address,
            PhoneNumberConfirme = true,
            CreatedAt = TimeHelper.GetDateTime(),
            UpdatedAt = TimeHelper.GetDateTime(),
        };

        var hasher = PasswordHasher.Hash(userRegisterDto.Password);
        user.PasswordHash = hasher.Hash;
        user.Salt = hasher.Salt;

        Role role = new Role();

       // var dbresult1 = await _roleRepository.CreateAsync();

        var dbResult = await _userRepository.CreateAsync(user);

        return dbResult ;
    }

    public async Task<(bool Result, int CachedMinutes)> ResetPasswordAsync(ForgotPassword dto)
    {
        var dbResult = await _userRepository.GetByPhoneAsync(dto.PhoneNumber);

        if (dbResult is null)
            throw new UserNotFoundException();

        UserRegisterDto userRegisterDto = new UserRegisterDto()
        {
            FirstName = dbResult.FirstName,
            LastName = dbResult.LastName,
            PhoneNumber = dto.PhoneNumber,
            Region = dbResult.Region,
            District = dbResult.District,
            Address = dbResult.Address,
            Password = dto.NewPassword,
        };

        if (_memoryCache.TryGetValue(Reset_CACHE_KEY + dto.PhoneNumber, out UserRegisterDto resetDto))
        {
            resetDto.PhoneNumber = resetDto.PhoneNumber;
            _memoryCache.Remove(dto.PhoneNumber);
        }
        else
        {
            _memoryCache.Set(Reset_CACHE_KEY + dto.PhoneNumber, userRegisterDto, TimeSpan.FromMinutes
                (CACHED_FOR_MINUTS_VEFICATION));
        }

        if (_memoryCache.TryGetValue(Reset_CACHE_KEY + dto.PhoneNumber, out UserRegisterDto registerDto))
        {
            VerificationDto verificationDto = new VerificationDto();
            verificationDto.Attempt = 0;
            verificationDto.CreatedAt = TimeHelper.GetDateTime();
            verificationDto.Code = 12345;//CodeGenerator.CodeGeneratorPhoneNumber();
            _memoryCache.Set(dto.PhoneNumber, verificationDto, TimeSpan.FromMinutes(CACHED_FOR_MINUTS_VEFICATION));

            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + dto.PhoneNumber,
                out VerificationDto OldverificationDto))
            {
                _memoryCache.Remove(dto.PhoneNumber);
            }

            _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + dto.PhoneNumber, verificationDto,
                TimeSpan.FromMinutes(VERIFICATION_MAXIMUM_ATTEMPTS));

            SmsSenderDto smsSenderDto = new SmsSenderDto();
            smsSenderDto.Title = "Green sale\n";
            smsSenderDto.Content = "Your verification code : " + verificationDto.Code;
            smsSenderDto.Recipent = dto.PhoneNumber.Substring(1);
            var result = true;//await _smsSender.SendAsync(smsSenderDto);

            if (result is true)
                return (Result: true, CachedVerificationMinutes: CACHED_FOR_MINUTS_VEFICATION);
            else
                return (Result: false, CACHED_FOR_MINUTS_VEFICATION: 0);
        }
        else
        {
            throw new ExpiredException();
        }
    }

    public async Task<(bool Result, string Token)> VerifyResetPasswordAsync(string phoneNumber, int code)
    {
        if (_memoryCache.TryGetValue(Reset_CACHE_KEY + phoneNumber, out UserRegisterDto userRegisterDto))
        {
            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + phoneNumber, out VerificationDto verificationDto))
            {
                if (verificationDto.Code == code)
                {
                    var dbcheck = await _userRepository.GetByPhoneAsync(phoneNumber);
                    var dbresult = await ResetAsync(dbcheck.Id, userRegisterDto);
                    if (dbresult > 0)
                    {
                        var result = await _userRepository.GetByPhoneAsync(phoneNumber);
                        UserRoleViewModel userRoleViewModel = new UserRoleViewModel()
                        {
                            Id = dbcheck.Id,
                            FirstName = result.FirstName,
                            LastName = result.LastName,
                            PhoneNumber = result.PhoneNumber,
                            Region = result.Region,
                            District = result.District,
                            Address = result.Address,
                            RoleName = "User",
                            CreatedAt = result.CreatedAt,
                            UpdatedAt = result.UpdatedAt,
                        };

                        string token = _tokenService.GenerateToken(userRoleViewModel);

                        return (Result: true, Token: token);
                    }
                    else
                        return (Result: false, Token: "");
                }
                else
                {
                    _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + phoneNumber);
                    verificationDto.Attempt++;
                    _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + phoneNumber, verificationDto,
                        TimeSpan.FromMinutes(CACHED_FOR_MINUTS_VEFICATION));

                    return (Result: false, Token: "");
                }
            }
            else throw new VerificationCodeExpiredException();
        }
        else throw new ExpiredException();
    }

    private async Task<int> ResetAsync(long id, UserRegisterDto userRegisterDto)
    {
        User user = new User()
        {
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            PhoneNumber = userRegisterDto.PhoneNumber,
            Region = userRegisterDto.Region,
            District = userRegisterDto.District,
            Address = userRegisterDto.Address,
            PhoneNumberConfirme = true,
            UpdatedAt = TimeHelper.GetDateTime(),
        };

        var hasher = PasswordHasher.Hash(userRegisterDto.Password);
        user.PasswordHash = hasher.Hash;
        user.Salt = hasher.Salt;
        var dbResult = await _userRepository.UpdateAsync(id, user);

        return dbResult;
    }

    public async Task<bool> CheckTokenAsync(AuthorizationDto token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(token.Authorization, validationParameters, out validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = "http://GreenSale.uz",
            ValidAudience = "GreenSale",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23f926fb-dcd2-49f4-8fe2-992aac18f08f")) // The same key as the one that generate the token
        };
    }
}
