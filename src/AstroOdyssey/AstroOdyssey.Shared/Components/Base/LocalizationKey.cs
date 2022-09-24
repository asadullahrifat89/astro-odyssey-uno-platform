namespace AstroOdyssey
{
    public class LocalizationKey
    {
        public LocalizationKey(string key, (string Culture, string Value)[] cultureValues)
        {
            Key = key;
            CultureValues = cultureValues;
        }

        public string Key { get; set; }

        public (string Culture, string Value)[] CultureValues { get; set; }
    }
}

