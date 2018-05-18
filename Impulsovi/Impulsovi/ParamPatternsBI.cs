using System;
using System.Collections.Generic;
using System.Text;

namespace Impulsovi
{
    public static class ParamPatternsBI
    {
        /// <summary>
        /// Url pro nutnou prvni inicializaci (TimeTable)
        /// </summary>
        public const string TimeTableRequestParams = "current=&airtime=&program=1048&blocks=&block=";

        private const string FullParamsPatternMarcelKocir1 = @"kbmg_id=&email=marcel.kocir@centrum.cz&jmeno=Marcel&prijmeni=Kočíř&psc=73532&vek=40&pohlavi=muz&tel=%2B420737731802&podminky=checkbox&captcha_code={2}&save=Registrovat&spmchk={0}&{1}=1";

        private const string FullParamsPatternMarcelKocir2= @"kbmg_id=&email=marcel.kocir@gmail.com&jmeno=Marcel&prijmeni=Kočíř&psc=73532&vek=40&pohlavi=muz&tel=%2B420777885392&podminky=checkbox&captcha_code={2}&save=Registrovat&spmchk={0}&{1}=1";

        private const string FullParamsPatternLenkaKocirova1 = @"kbmg_id=&email=jadoledo@centrum.cz&jmeno=Lenka&prijmeni=Kočířová&psc=73532&vek=39&pohlavi=zena&tel=%2B420777891025&podminky=checkbox&captcha_code={2}&save=Registrovat&spmchk={0}&{1}=1";

        private const string FullParamsPatternLenkaKocirova2 = @"kbmg_id=&email=lenka.kocirova@dhl.com&jmeno=Lenka&prijmeni=Kočířová&psc=73532&vek=39&pohlavi=zena&tel=%2B420739656696&podminky=checkbox&captcha_code={2}&save=Registrovat&spmchk={0}&{1}=1";

        public static List<string> FullParamsPatternList = new List<string>()
        {
            FullParamsPatternMarcelKocir1,
            FullParamsPatternMarcelKocir2,
            FullParamsPatternLenkaKocirova1,
            FullParamsPatternLenkaKocirova2
        };

    }
}
