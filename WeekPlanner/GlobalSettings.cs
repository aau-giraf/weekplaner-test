using IO.Swagger.Model;

namespace WeekPlanner
{
	public class GlobalSettings
	{
		public const string DefaultEndpoint = "http://localhost:5000";

		private string _baseEndpoint;
		private static readonly GlobalSettings _instance = new GlobalSettings();

		public GlobalSettings()
		{
			BaseEndpoint = DefaultEndpoint;
		}

		public static GlobalSettings Instance
		{
			get { return _instance; }
		}

		public string BaseEndpoint
		{
			get { return _baseEndpoint; }
			set
			{
				_baseEndpoint = value;
			}
		}

		public bool UseMocks = false;

		public string DepartmentAuthToken { get; set; }

		public string CitizenAuthToken { get; set; }

		public DepartmentNameDTO Department { get; set; } = new DepartmentNameDTO { Name = "Egebakken" };
	}
}
