namespace AstroOdyssey
{
    public class LocalizationKey
    {
        public LocalizationKey(string key, CultureValue[] cultureValues)
        {
            Key = key;
            CultureValues = cultureValues;
        }

        public string Key { get; set; }

        public CultureValue[] CultureValues { get; set; }
    }

    public class CultureValue
    {
        public CultureValue(string culture, string value)
        {
            Culture = culture;
            Value = value;
        }

        public string Culture { get; set; }

        public string Value { get; set; }
    }
}

