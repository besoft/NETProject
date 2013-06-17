using System;
using Zcu.StudentEvaluator.Core.Data;
using Zcu.StudentEvaluator.Core.Data.Schema;
using Zcu.StudentEvaluator.Domain.Test;

namespace TestApplication
{
     /// <summary>
    /// Tento 4. program testuje zaklad hodnoceni studentu na predmetu.
    /// </summary>
    /// <remarks>
    ///Lze si to predstavit jako Excelovskou tabulku MxN, kde N je pocet studentu (pocet radek tabulky) a M je pocet sloupcu, 
    ///ktere mam k dispozici, pricemz v dobe prekladu jsou znamy jen 3 sloupce (osobni cislo, jmeno, prijmeni)
    ///a ostatni sloupce jsou stanoveny v dobe behu (napr. z konfiguracniho souboru), kazdy z techto sloupcu odpovida hodnoceni studentu v nejake kategorii.
    ///Protoze definice sloupcu (jmeno kategorie, min a max mozny pocet bodu) je spolecny pro kazdeho studenta, je vhodne definici udrzovat v pameti jen jednou, 
    ///jen jedna instance. Vlastni hodnoceni (pocet bodu a jejich zduvodneni) jsou uchovavany samozrejme pro kazdeho studenta.
    ///
    /// Tento 4. navrh vychazi z prvniho a dosahuje nasledujicich zlepseni:
    /// 1) Zavedeny kolekce pro definice a pro StudentCourseEvaluation, coz umoznuje disponovat kolekci studentu, ktera muze byt poskytovana
    /// prostrednictvim rozhrani IStudentsRepository tak, ze obsahuje vsechny studenty, jen studenty, kteri prosli, ...    
    /// 2) Tridy Student, StudentCourseEvaluation upraveny tak, aby nebylo mozne zadat "null" pro klicove parametry
    /// 3) Odstraneno nekolik bugu odhalenych na zaklade vypracovanychh unit testu
    /// 4) Pridano nekolik unit testu, ktere overuji vsechny tridy, pokryti kodu je 100% az na CourseEvaluation
    /// 
    /// Nedostatky:
    /// 1) Navrh je nekonzistentni: zatimco v pripade Studentu (a jejich hodnoceni) je oddelena kolekce od filtrovani teto kolekce,
    /// CourseEvaluation spojuje dohromady oboje: kolekci hodnoceni a metody pro filtrovani a praci s touto kolekci
    /// 2) Chybi funkcionalita pro vyber hodnoceni dle pozadavku (dle nejake definice)
    /// 3) Chybi funkcionalita pro filtrovani studentu
    /// 
    /// Ukazane principy: observable kolekce, unit testy, 
    /// </remarks>    
    class Program
    {

        static void Main(string[] args)
        {
            var repo = new TestRepository();

            Console.WriteLine("Number of students: " + repo.StudentsCourseEvaluation.Count);
            ListStudents(repo);

            CourseEvaluation student0 = repo.StudentsCourseEvaluation[0].Evaluation;
            CourseEvaluation student1 = repo.StudentsCourseEvaluation[1].Evaluation;

            EvaluationValue val0 = student0.Evaluations[0].Value;
            val0.Points = 5.5m; val0.Reason = "Bad design";

            EvaluationValue val1 = student0.Evaluations[1].Value;
            val1.Points = 4m; val1.Reason = "Bad implementation";

            student0.Evaluations[3].Value.Points = 3m;

            student1.Evaluations[0].Value.Points = 15m;

            ListStudents(repo);

            //zmena definice hodnoceni                        
            repo.StudentsCourseEvaluation[2].Evaluation.EvaluationDefinitions.RemoveAt(2);
            repo.StudentsCourseEvaluation[2].Evaluation.EvaluationDefinitions.Move(0, 1);
            repo.StudentsCourseEvaluation[2].Evaluation.EvaluationDefinitions.Add(
                new EvaluationDefinition() {  Name = "Other", MinPoints=0m, MaxPoints=5m});

            ListStudents(repo);

            repo.StudentsCourseEvaluation[2].Evaluation.EvaluationDefinitions[2].MinPoints = 3.5m;
            student1.Evaluations[3].Value.Points = 4m;

            ListStudents(repo);

            repo.StudentsCourseEvaluation[2].Evaluation.EvaluationDefinitions[3] = null;
            ListStudents(repo);

            Console.ReadLine();
        }

        private static void ListStudents(TestRepository repo)
        {
            foreach (var st in repo.StudentsCourseEvaluation)
            {
                Console.WriteLine(st.ToString());
            }

            Console.WriteLine("===================");
        }
    }
}
