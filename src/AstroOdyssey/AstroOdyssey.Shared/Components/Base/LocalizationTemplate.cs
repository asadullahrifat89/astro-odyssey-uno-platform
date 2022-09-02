namespace AstroOdyssey
{
    public class LocalizationTemplate 
    {
        public LocalizationTemplate(string key, (string Culture, string Value)[] cultureValues)
        {
            Key = key;
            CultureValues = cultureValues;
        }

        public string Key { get; set; }

        public (string Culture, string Value)[] CultureValues { get; set; }
    }
}

