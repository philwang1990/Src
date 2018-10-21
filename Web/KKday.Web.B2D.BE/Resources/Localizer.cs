namespace Resources
{
	using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.Loader;
    using System.Text.RegularExpressions;

    public interface ILocalizer
    {
		string Culture { get; set; }

		Text Text { get; }

        string GetString(Type category, string resourceKey);

        string GetString(string category, string resourceKey);

        string GetString(Type category, string resourceKey, string culture);

        string GetString(string category, string resourceKey, string culture);
    }

    public class Localizer : ILocalizer
    {
        private const string DefaultCulture = "zh-TW";
        private static readonly Lazy<Dictionary<string, ResourceManager>> _resources = new Lazy<Dictionary<string, ResourceManager>>(LoadResourceManager);
		private static string _assemblyPath;
        private string _culture;
        private Text _text;

		public Localizer() {
			_assemblyPath = Assembly.GetEntryAssembly().Location;
		}

		public Localizer(string assemblyPath) {
			_assemblyPath = assemblyPath;
		}
            
        #region ILocalizer

		public string Culture
        {
            get
            {
                if (string.IsNullOrEmpty(_culture))
                {
                    _culture = DefaultCulture;
                }
                return _culture;
            }
            set
            {
                var culture = value;
                if (Regex.IsMatch(culture, @"^[A-Za-z]{2}-[A-Za-z]{2}$"))
                {
                    _culture = culture;
                }
                else
                {
                    _culture = DefaultCulture;
                }
            }
        }

		public Text Text { get { if (_text == null) { _text = new Text(this); } return _text; } }

        public string GetString(Type category, string resourceKey)
        {
            return GetString(category.Name.ToString(), resourceKey);
        }

        public string GetString(string category, string resourceKey)
        {
            return GetString(category, resourceKey, _culture);
        }

        public string GetString(Type category, string resourceKey, string culture)
        {
            return GetString(category.Name.ToString(), resourceKey, culture);
        }

        public string GetString(string category, string resourceKey, string culture)
        {
            var resource = GetResource($"{category}.{culture}") ?? GetResource($"{category}.{DefaultCulture}");
            if (resource == null)
            {
                return resourceKey;
            }
            else
            {
                return resource.GetString(resourceKey);
            }
        }

        #endregion ILocalizer

        #region Private Methods

        private static Dictionary<string, ResourceManager> LoadResourceManager()
        {
            var directory = Path.GetDirectoryName(_assemblyPath);
            var files = Directory.GetFiles(directory, "*.resources.dll", SearchOption.AllDirectories);
            
            var resources = new Dictionary<string, ResourceManager>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var file in files)
            {
                var culture = Path.GetFileName(Path.GetDirectoryName(file));
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                foreach(var resourceName in assembly.GetManifestResourceNames().Select(s=> Regex.Replace(s, ".resources$", "")))
                {
                    var category = Regex.Match(resourceName, $".*Resources\\.(.*)\\.{culture}").Groups[1].Value;
                    var resourceManager = new ResourceManager(resourceName, assembly);
                    resources.Add($"{category}.{culture}", resourceManager);
                }
            }

            return resources;
        }

        private ResourceManager GetResource(string key)
        {
            if (_resources.Value.Keys.Contains(key))
            {
                return _resources.Value[key];
            }
            return null;
        }

        #endregion
    }

    public abstract class ResourceBase
    {
        protected ResourceBase(ILocalizer localizer)
        {
            Localizer = localizer;
        }

        protected ILocalizer Localizer { get; private set; }

        protected string GetString(string resourceKey)
        {
            return Localizer.GetString(GetType(), resourceKey);
        }
    }

    public class Text : ResourceBase
    {
        public Text(ILocalizer localizer) : base(localizer)
        {
        }

		public string CURR_AUD { get { return GetString("CURR_AUD"); } }

		public string CURR_CAD { get { return GetString("CURR_CAD"); } }

		public string CURR_CNY { get { return GetString("CURR_CNY"); } }

		public string CURR_EUR { get { return GetString("CURR_EUR"); } }

		public string CURR_GBP { get { return GetString("CURR_GBP"); } }

		public string CURR_HKD { get { return GetString("CURR_HKD"); } }

		public string CURR_IDR { get { return GetString("CURR_IDR"); } }

		public string CURR_JPY { get { return GetString("CURR_JPY"); } }

		public string CURR_KRW { get { return GetString("CURR_KRW"); } }

		public string CURR_MYR { get { return GetString("CURR_MYR"); } }

		public string CURR_NZD { get { return GetString("CURR_NZD"); } }

		public string CURR_PHP { get { return GetString("CURR_PHP"); } }

		public string CURR_SGD { get { return GetString("CURR_SGD"); } }

		public string CURR_THB { get { return GetString("CURR_THB"); } }

		public string CURR_TWD { get { return GetString("CURR_TWD"); } }

		public string CURR_USD { get { return GetString("CURR_USD"); } }

		public string CURR_VND { get { return GetString("CURR_VND"); } }

		public string KKday_CopyRight { get { return GetString("KKday_CopyRight"); } }

		public string Add { get { return GetString("Add"); } }

		public string Update { get { return GetString("Update"); } }

		public string Edit { get { return GetString("Edit"); } }

		public string Delete { get { return GetString("Delete"); } }

		public string ID { get { return GetString("ID"); } }

		public string Name { get { return GetString("Name"); } }

		public string UserName { get { return GetString("UserName"); } }

		public string Country { get { return GetString("Country"); } }

		public string Status { get { return GetString("Status"); } }

		public string Sorting { get { return GetString("Sorting"); } }

		public string Promotion { get { return GetString("Promotion"); } }

		public string Search { get { return GetString("Search"); } }

		public string Query { get { return GetString("Query"); } }

		public string Data_Not_Found { get { return GetString("Data_Not_Found"); } }

		public string Yes { get { return GetString("Yes"); } }

		public string No { get { return GetString("No"); } }

		public string Enable { get { return GetString("Enable"); } }

		public string Disable { get { return GetString("Disable"); } }

		public string Account { get { return GetString("Account"); } }

		public string Email { get { return GetString("Email"); } }

		public string Title_Distributor { get { return GetString("Title_Distributor"); } }

		public string Title_VoucherApply { get { return GetString("Title_VoucherApply"); } }

		public string Title_Order { get { return GetString("Title_Order"); } }

		public string Title_B2dAccount { get { return GetString("Title_B2dAccount"); } }

		public string Title_B2dApiAccount { get { return GetString("Title_B2dApiAccount"); } }

		public string Title_PriceBlacklist { get { return GetString("Title_PriceBlacklist"); } }

		public string Title_Promotion { get { return GetString("Title_Promotion"); } }

		public string UserType { get { return GetString("UserType"); } }

		public string Company_Name { get { return GetString("Company_Name"); } }

		public string Company_Status_00 { get { return GetString("Company_Status_00"); } }

		public string Company_Status_01 { get { return GetString("Company_Status_01"); } }

		public string Company_Status_02 { get { return GetString("Company_Status_02"); } }

		public string Company_Status_03 { get { return GetString("Company_Status_03"); } }
    }
}
