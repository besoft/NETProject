using System;
using Zcu.StudentEvaluator.Domain.Test;

namespace TestApplication
{
     /// <summary>
    /// Tento program vychazi ze zakladu hodnoceni studentu na predmetu.
    /// </summary>
    /// <remarks>
    ///Lze si to predstavit jako Excelovskou tabulku MxN, kde N je pocet studentu (pocet radek tabulky) a M je pocet sloupcu, 
    ///ktere mam k dispozici, pricemz v dobe prekladu jsou znamy jen 3 sloupce (osobni cislo, jmeno, prijmeni)
    ///a ostatni sloupce jsou stanoveny v dobe behu (napr. z konfiguracniho souboru), kazdy z techto sloupcu odpovida hodnoceni studentu v nejake kategorii.
    ///Protoze definice sloupcu (jmeno kategorie, min a max mozny pocet bodu) je spolecny pro kazdeho studenta, je vhodne definici udrzovat v pameti jen jednou, 
    ///jen jedna instance. Vlastni hodnoceni (pocet bodu a jejich zduvodneni) jsou uchovavany samozrejme pro kazdeho studenta.
    ///
    /// Tento druhy navrh vychazi z prvniho a dosahuje nasledujicich zlepseni:
    /// 1) Nektere tridy byly s pouzitim Refaktoringu prejmenovany pro lepsi citelnost, napr. EvaluationItemSchem -> EvaluationDefinition, ...
    /// 2) Udaje o studentovi a udaje o jeho hodnoceni jsou oddeleny
    /// 3) Hodnoceni a definice hodoceni (schema) jsou volne provazany, takze lze snadno zjistit, zda student splnil podminky, brat v uvahu soucet, ...
    ///     Pozn. pro tento ucel bylo nezbytne zmenit EvaluationDefinition z hodnotoveho datoveho typu (struct) na referencni (class)
    /// 4) Schema lze snadno modifikovat, protoze se jedna o kolekci (ackoliv vnitrni implementace v Repo je pres pole)
    /// 
    /// Nedostatky:
    /// 1) Schema (definici) hodnoceni muze nyni obsahovat null polozky (povede k padu)
    /// 2) Schema (definici) hodnoceni lze zmenit, aniz by se o tom prislusna instance CourseEvaluation dozvedela    
    /// 
    /// Ukazane principy: kolekce, LINQ, refactoring
    /// </remarks>    
    class Program
    {

        static void Main(string[] args)
        {
            var repo = new TestRepository();

            Console.WriteLine("Number of students: " + repo.StudentsCourseEvaluation.Length);
            foreach (var st in repo.StudentsCourseEvaluation)
            {
                Console.WriteLine(st.ToString());
            }

            repo.StudentsCourseEvaluation[0].Evaluation.Evaluations[0].Points = 5.5m;
            repo.StudentsCourseEvaluation[0].Evaluation.Evaluations[0].Reason = "Bad design";

            repo.StudentsCourseEvaluation[0].Evaluation.Evaluations[1].Points = 4m;
            repo.StudentsCourseEvaluation[0].Evaluation.Evaluations[1].Reason = "Bad implementation";

            repo.StudentsCourseEvaluation[0].Evaluation.Evaluations[2].Points = 2m;

            repo.StudentsCourseEvaluation[1].Evaluation.Evaluations[0].Points = 15m;

            foreach (var st in repo.StudentsCourseEvaluation)
            {
                Console.WriteLine(st.ToString());
            }

            //zmena definice hodnoceni
            repo.StudentsCourseEvaluation[2].Evaluation.EvaluationDefinitions[2] = null;

            //Nefunguje pro pole => nutno upravit TestRepository
            repo.StudentsCourseEvaluation[2].Evaluation.EvaluationDefinitions.RemoveAt(2);

            foreach (var st in repo.StudentsCourseEvaluation)
            {
                Console.WriteLine(st.ToString());
            }            

            Console.ReadLine();
        }
    }
}
