using Terminal.Gui;

namespace TerminalGUILibrary
{
    public class Features
    {
        private static int _maxFeaturesNameLen = 30;

        public Window Win { get; set; }
        public static List<string> GetAllCategories ()
        {
            List<string> categories = new List<string>();
 
			foreach(Type type in typeof (Features).Assembly.GetTypes()
			.Where (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf (typeof(Features))))
			{
				List<System.Attribute> attrs = System.Attribute.GetCustomAttributes (type).ToList ();
				categories = categories.Union (attrs.Where (a => a is FeaturesCategory).Select (a => ((FeaturesCategory)a).Name)).ToList ();
			}
            return categories;
        }

        public static List<Features> GetAllFeatures ()
        {
            List<Features> objects = new List<Features>();
 
			foreach(Type type in typeof (Features).Assembly.ExportedTypes
			.Where (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf (typeof(Features))))
			{
				var features = (Features)Activator.CreateInstance (type);
				objects.Add (features);
				_maxFeaturesNameLen = Math.Max (_maxFeaturesNameLen, features.GetName ().Length + 1);
			}
            return objects.OrderBy(s => s.GetName()).ToList();
        }

		// make object return metadata (name+description) instead of class name
		// ToString is called when instance is created
		public override string ToString () => $"{GetName ().PadRight(_maxFeaturesNameLen)}{GetDescription ()}";

		public virtual void Init ()
		{
			Application.Init ();

			Win = new Window ($"CTRL-Q to Close - Feature: {GetName ()}") {
				X = 0,
				Y = 0,
				Width = Dim.Fill (),
				Height = Dim.Fill (),
			};
			Application.Top.Add (Win);
		}

		// override Setup() in subclass instead of constructor to make sure UI elements is only called once.
		public virtual void Setup ()
		{
		}

		public virtual void Run ()
		{
			Application.Run (Application.Top);
		}

		public virtual void RequestStop ()
		{
			Application.RequestStop ();
		}

        [System.AttributeUsage (System.AttributeTargets.Class)]
		public class FeaturesMetadata : System.Attribute {

			public string Name { get; set; }
			public string Description { get; set; }

			public FeaturesMetadata (string Name, string Description)
			{
				this.Name = Name;
				this.Description = Description;
			}

			public static string GetName (Type t) => ((FeaturesMetadata)System.Attribute.GetCustomAttributes (t) [0]).Name;
			public static string GetDescription (Type t) => ((FeaturesMetadata)System.Attribute.GetCustomAttributes (t) [0]).Description;
		}

		public string GetName () => FeaturesMetadata.GetName (this.GetType ());
		public string GetDescription () => FeaturesMetadata.GetDescription (this.GetType ());


		[System.AttributeUsage (System.AttributeTargets.Class, AllowMultiple = true)]
		public class FeaturesCategory : System.Attribute {
			public string Name { get; set; }

			public FeaturesCategory (string Name) => this.Name = Name;
			public static string GetName (Type t) => ((FeaturesCategory)System.Attribute.GetCustomAttributes (t) [0]).Name;

			public static List<string> GetCategories (Type t) => System.Attribute.GetCustomAttributes (t)
				.ToList ()
				.Where (a => a is FeaturesCategory)
				.Select (a => ((FeaturesCategory)a).Name)
				.ToList ();
		}

		public List<string> GetCategories () => FeaturesCategory.GetCategories (this.GetType ());  
    }
}