namespace GreenSale.Persistence.Validators;

public class PhoneNumberValidator
{
    public static bool IsValid(string phoneNumber)
    {
        if (phoneNumber.Length != 13) return false;

        if (phoneNumber.StartsWith("+998") == false) return false;

        for (int i = 1; i < phoneNumber.Length; i++)
        {
            if (char.IsDigit(phoneNumber[i]) == true) continue;
            else
                return false;
        }

        return true;
    }
}
