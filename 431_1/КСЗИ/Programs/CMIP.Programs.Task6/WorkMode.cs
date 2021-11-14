namespace CMIP.Programs.Task6
{
    internal enum WorkMode
    {
        NONE = 0,
        ENCRYPT_KEY1 = 1,
        ENCRYPT_KEY2 = 2,
        CALCULATE_SUPER_KEY = 3,
        ENCRYPT_SUPER_KEY = 4,
        DECRYPT_KEY1 = 5,
        DECRYPT_KEY2 = 6,
        DECRYPT_SUPER_KEY = 7,
        CLOSE_PROGRAM = 0
    }
}
