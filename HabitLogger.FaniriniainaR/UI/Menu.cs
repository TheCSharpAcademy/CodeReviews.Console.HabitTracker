using HabitLogger.Services;

Namespace HabitLogger.UI
{
    public static class Menu
    {
        public static voId Show()
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

        private static voId HandleAction(string input)
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
                        Console.WriteLine("Option invalIde. Veuillez réessayer.");
                        break;
                }
            }
            catch (Exception ex)
            {
                AfficherErreur(ex.Message);
            }
        }

        private static voId AjouterHabitude()
        {
            Console.WriteLine("\nAJOUT D'UNE HABITUDE");
            Console.Write("Entrez la Date (AAAA-MM-JJ) : ");
            string Date = Console.ReadLine();

            if (!DateTime.TryParse(Date, out _))
                throw new Exception("Format de Date invalIde.");

            Console.Write("Entrez la quantité : ");
            if (!int.TryParse(Console.ReadLine(), out int Quantity))
                throw new Exception("Quantité invalIde.");

            var types = HabitService.GetHabitTypes();
            if (types.Count == 0)
                throw new Exception("Aucun type d’habitude disponible. Veuillez en ajouter un d’abord.");

            Console.WriteLine("\nTypes disponibles :");
            foreach (var t in types)
                Console.WriteLine($" {t.Id} - {t.Name} ({t.Unit})");

            Console.Write("Entrez l'Id du type d’habitude : ");
            if (!int.TryParse(Console.ReadLine(), out int typeId) || !types.Any(t => t.Id == typeId))
                throw new Exception("Id de type invalIde.");

            HabitService.AddHabit(Date, Quantity, typeId);
        }

        private static voId VoirToutesLesHabitudes()
        {
            HabitService.ShowAllHabits();
        }

        private static voId VoirHabitudesParType()
        {
            Console.Write("\nEntrez le nom du type d’habitude : ");
            string type = Console.ReadLine();
            HabitService.ShowHabitsByType(type);
        }

        private static voId VoirHabitudesParPeriode()
        {
            Console.Write("Date de début (AAAA-MM-JJ) : ");
            string start = Console.ReadLine();
            Console.Write("Date de fin (AAAA-MM-JJ) : ");
            string end = Console.ReadLine();

            HabitService.ShowHabitsByDateRange(start, end);
        }

        private static voId VoirStatistiques()
        {
            HabitService.ShowStatistics();
        }

        private static voId AjouterTypeHabitude()
        {
            Console.WriteLine("\nAJOUT D'UN TYPE D’HABITUDE");
            Console.Write("Entrez le nom du type : ");
            string Name = Console.ReadLine();
            Console.Write("Entrez l’unité de mesure (ex: min, km, verres...) : ");
            string unit = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(unit))
                throw new Exception("Nom ou unité invalIde.");

            HabitService.AddHabitType(Name, unit);
        }

        private static voId Quitter()
        {
            Console.WriteLine("Au revoir !");
            Environment.Exit(0);
        }

        private static voId AfficherErreur(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erreur : {message}");
            Console.ResetColor();
        }
    }
}
