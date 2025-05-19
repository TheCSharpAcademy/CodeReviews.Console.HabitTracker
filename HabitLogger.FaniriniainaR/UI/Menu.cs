using HabitLogger.Services;

namespace HabitLogger.UI
{
    public static class Menu
    {
        public static void Show()
        {
            while (true)
            {
                Console.WriteLine("\nMENU PRINCIPAL :");
                Console.WriteLine("1. Ajouter une habitude");
                Console.WriteLine("2. Voir toutes les habitudes");
                Console.WriteLine("3. Voir les habitudes par type");
                Console.WriteLine("4. Voir les habitudes par période");
                Console.WriteLine("5. Voir les statistiques");
                Console.WriteLine("6. Ajouter un type d’habitude");
                Console.WriteLine("7. Quitter");
                Console.Write("Choisissez une option : ");
                string input = Console.ReadLine();

                HandleAction(input);
            }
        }

        private static void HandleAction(string input)
        {
            try
            {
                switch (input)
                {
                    case "1":
                        AjouterHabitude();
                        break;
                    case "2":
                        VoirToutesLesHabitudes();
                        break;
                    case "3":
                        VoirHabitudesParType();
                        break;
                    case "4":
                        VoirHabitudesParPeriode();
                        break;
                    case "5":
                        VoirStatistiques();
                        break;
                    case "6":
                        AjouterTypeHabitude();
                        break;
                    case "7":
                        Quitter();
                        break;
                    default:
                        Console.WriteLine("Option invalide. Veuillez réessayer.");
                        break;
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex.Message);
            }
        }

        private static void AjouterHabitude()
        {
            Console.WriteLine("\nAJOUT D'UNE HABITUDE");
            Console.Write("Entrez la date (AAAA-MM-JJ) : ");
            string date = Console.ReadLine();

            if (!DateTime.TryParse(date, out _))
                throw new Exception("Format de date invalide.");

            Console.Write("Entrez la quantité : ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
                throw new Exception("Quantité invalide.");

            var types = HabitService.GetHabitTypes();
            if (types.Count == 0)
                throw new Exception("Aucun type d’habitude disponible. Veuillez en ajouter un d’abord.");

            Console.WriteLine("\nTypes disponibles :");
            foreach (var t in types)
                Console.WriteLine($" {t.Id} - {t.Name} ({t.Unit})");

            Console.Write("Entrez l'ID du type d’habitude : ");
            if (!int.TryParse(Console.ReadLine(), out int typeId) || !types.Any(t => t.Id == typeId))
                throw new Exception("ID de type invalide.");

            HabitService.AddHabit(date, quantity, typeId);
        }

        private static void VoirToutesLesHabitudes()
        {
            HabitService.ShowAllHabits();
        }

        private static void VoirHabitudesParType()
        {
            Console.Write("\nEntrez le nom du type d’habitude : ");
            string type = Console.ReadLine();
            HabitService.ShowHabitsByType(type);
        }

        private static void VoirHabitudesParPeriode()
        {
            Console.Write("Date de début (AAAA-MM-JJ) : ");
            string start = Console.ReadLine();
            Console.Write("Date de fin (AAAA-MM-JJ) : ");
            string end = Console.ReadLine();

            HabitService.ShowHabitsByDateRange(start, end);
        }

        private static void VoirStatistiques()
        {
            HabitService.ShowStatistics();
        }

        private static void AjouterTypeHabitude()
        {
            Console.WriteLine("\nAJOUT D'UN TYPE D’HABITUDE");
            Console.Write("Entrez le nom du type : ");
            string name = Console.ReadLine();
            Console.Write("Entrez l’unité de mesure (ex: min, km, verres...) : ");
            string unit = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(unit))
                throw new Exception("Nom ou unité invalide.");

            HabitService.AddHabitType(name, unit);
        }

        private static void Quitter()
        {
            Console.WriteLine("Au revoir !");
            Environment.Exit(0);
        }

        private static void AfficherErreur(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erreur : {message}");
            Console.ResetColor();
        }
    }
}
