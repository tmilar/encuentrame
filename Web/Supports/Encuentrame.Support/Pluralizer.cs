using System;
using System.Collections.Generic;

namespace Encuentrame.Support
{
    public static class Pluralizer
    {
        #region public APIs

        public static string ToPlural(this string noun)
        {
            return AdjustCase(ToPluralInternal(noun), noun);
        }

        public static string ToSingular(this string noun)
        {
            return AdjustCase(ToSingularInternal(noun), noun);
        }

        public static bool IsPluralOf(this string plural, string singular)
        {
            return String.Compare(ToSingularInternal(plural), singular, StringComparison.OrdinalIgnoreCase) == 0;
        }

        #endregion

        #region Special Words Table

        private static readonly string[] specialWordsStringTable = new[]
                                                                       {
                                                                           "agendum", "agenda", "",
                                                                           "albino", "albinos", "",
                                                                           "alga", "algae", "",
                                                                           "alumna", "alumnae", "",
                                                                           "apex", "apices", "apexes",
                                                                           "archipelago", "archipelagos", "",
                                                                           "bacterium", "bacteria", "",
                                                                           "beef", "beefs", "beeves",
                                                                           "bison", "", "",
                                                                           "brother", "brothers", "brethren",
                                                                           "candelabrum", "candelabra", "",
                                                                           "carp", "", "",
                                                                           "casino", "casinos", "",
                                                                           "child", "children", "",
                                                                           "chassis", "", "",
                                                                           "chinese", "", "",
                                                                           "clippers", "", "",
                                                                           "cod", "", "",
                                                                           "codex", "codices", "",
                                                                           "commando", "commandos", "",
                                                                           "corps", "", "",
                                                                           "cortex", "cortices", "cortexes",
                                                                           "cow", "cows", "kine",
                                                                           "criterion", "criteria", "",
                                                                           "datum", "data", "",
                                                                           "debris", "", "",
                                                                           "diabetes", "", "",
                                                                           "ditto", "dittos", "",
                                                                           "djinn", "", "",
                                                                           "dynamo", "", "",
                                                                           "elk", "", "",
                                                                           "embryo", "embryos", "",
                                                                           "ephemeris", "ephemeris", "ephemerides",
                                                                           "erratum", "errata", "",
                                                                           "extremum", "extrema", "",
                                                                           "fiasco", "fiascos", "",
                                                                           "fish", "fishes", "fish",
                                                                           "flounder", "", "",
                                                                           "focus", "focuses", "foci",
                                                                           "fungus", "fungi", "funguses",
                                                                           "gallows", "", "",
                                                                           "genie", "genies", "genii",
                                                                           "ghetto", "ghettos", "",
                                                                           "graffiti", "", "",
                                                                           "headquarters", "", "",
                                                                           "herpes", "", "",
                                                                           "homework", "", "",
                                                                           "index", "indices", "indexes",
                                                                           "inferno", "infernos", "",
                                                                           "japanese", "", "",
                                                                           "jumbo", "jumbos", "",
                                                                           "latex", "latices", "latexes",
                                                                           "lingo", "lingos", "",
                                                                           "mackerel", "", "",
                                                                           "macro", "macros", "",
                                                                           "manifesto", "manifestos", "",
                                                                           "measles", "", "",
                                                                           "money", "moneys", "monies",
                                                                           "mongoose", "mongooses", "mongoose",
                                                                           "mumps", "", "",
                                                                           "murex", "murecis", "",
                                                                           "mythos", "mythos", "mythoi",
                                                                           "news", "", "",
                                                                           "octopus", "octopuses", "octopodes",
                                                                           "ovum", "ova", "",
                                                                           "ox", "ox", "oxen",
                                                                           "photo", "photos", "",
                                                                           "pincers", "", "",
                                                                           "pliers", "", "",
                                                                           "pro", "pros", "",
                                                                           "rabies", "", "",
                                                                           "radius", "radiuses", "radii",
                                                                           "rhino", "rhinos", "",
                                                                           "salaryinfo", "salariesInfo", "",
                                                                           "salmon", "", "",
                                                                           "scissors", "", "",
                                                                           "series", "", "",
                                                                           "shears", "", "",
                                                                           "silex", "silices", "",
                                                                           "simplex", "simplices", "simplexes",
                                                                           "soliloquy", "soliloquies", "soliloquy",
                                                                           "species", "", "",
                                                                           "stratum", "strata", "",
                                                                           "status", "statusses", "",
                                                                           "educationstatus", "educationStatuses", "",
                                                                           "maritalstatus", "maritalStatuses", "",
                                                                           "swine", "", "",
                                                                           "trout", "", "",
                                                                           "tuna", "", "",
                                                                           "vertebra", "vertebrae", "",
                                                                           "vertex", "vertices", "vertexes",
                                                                           "vortex", "vortices", "vortexes",
                                                                           "franchiseoptions", "franchiseoptions",""
                                                                       };

        #endregion

        #region Suffix Rules Table

        private static readonly string[] suffixRulesStringTable = new[]
                                                                      {
                                                                          "ch", "ches",
                                                                          "sh", "shes",
                                                                          "ss", "sses",
                                                                          "ay", "ays",
                                                                          "ey", "eys",
                                                                          "iy", "iys",
                                                                          "oy", "oys",
                                                                          "uy", "uys",
                                                                          "y", "ies",
                                                                          "ao", "aos",
                                                                          "eo", "eos",
                                                                          "io", "ios",
                                                                          "oo", "oos",
                                                                          "uo", "uos",
                                                                          "o", "oes",
                                                                          "cis", "ces",
                                                                          "sis", "ses",
                                                                          "xis", "xes",
                                                                          "louse", "lice",
                                                                          "mouse", "mice",
                                                                          "zoon", "zoa",
                                                                          "man", "men",
                                                                          "deer", "deer",
                                                                          "fish", "fish",
                                                                          "sheep", "sheep",
                                                                          "itis", "itis",
                                                                          "ois", "ois",
                                                                          "pox", "pox",
                                                                          "ox", "oxes",
                                                                          "foot", "feet",
                                                                          "goose", "geese",
                                                                          "tooth", "teeth",
                                                                          "alf", "alves",
                                                                          "elf", "elves",
                                                                          "olf", "olves",
                                                                          "arf", "arves",
                                                                          "leaf", "leaves",
                                                                          "nife", "nives",
                                                                          "life", "lives",
                                                                          "wife", "wives"
                                                                      };

        #endregion

        #region Implementation Details

        private static readonly Dictionary<string, Word> specialPlurals;
        private static readonly Dictionary<string, Word> specialSingulars;
        private static readonly List<SuffixRule> suffixRules;

        static Pluralizer()
        {
            // populate lookup tables for special words
            specialSingulars = new Dictionary<string, Word>(StringComparer.OrdinalIgnoreCase);
            specialPlurals = new Dictionary<string, Word>(StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < specialWordsStringTable.Length; i += 3)
            {
                var s = specialWordsStringTable[i];
                var p = specialWordsStringTable[i + 1];
                var p2 = specialWordsStringTable[i + 2];

                if (string.IsNullOrEmpty(p))
                {
                    p = s;
                }

                var w = new Word(s, p);

                specialSingulars.Add(s, w);
                specialPlurals.Add(p, w);

                if (!string.IsNullOrEmpty(p2))
                {
                    specialPlurals.Add(p2, w);
                }
            }

            // populate suffix rules list
            suffixRules = new List<SuffixRule>();

            for (var i = 0; i < suffixRulesStringTable.Length; i += 2)
            {
                var singular = suffixRulesStringTable[i];
                var plural = suffixRulesStringTable[i + 1];
                suffixRules.Add(new SuffixRule(singular, plural));
            }
        }

        private static string ToPluralInternal(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // lookup special words
            Word word;

            if (specialSingulars.TryGetValue(s, out word))
            {
                return word.Plural;
            }

            // apply suffix rules
            string plural;

            foreach (var rule in suffixRules)
            {
                if (rule.TryToPlural(s, out plural))
                {
                    return plural;
                }
            }

            // apply the default rule
            return s + "s";
        }

        private static string ToSingularInternal(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // lookup special words
            Word word;

            if (specialPlurals.TryGetValue(s, out word))
            {
                return word.Singular;
            }

            // apply suffix rules
            string singular;

            foreach (var rule in suffixRules)
            {
                if (rule.TryToSingular(s, out singular))
                {
                    return singular;
                }
            }

            // apply the default rule
            if (s.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                return s.Substring(0, s.Length - 1);
            }

            return s;
        }

        private static string AdjustCase(string s, string template)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            // determine the type of casing of the template string
            var foundUpperOrLower = false;
            var allLower = true;
            var allUpper = true;
            var firstUpper = false;

            for (var i = 0; i < template.Length; i++)
            {
                if (Char.IsUpper(template[i]))
                {
                    if (i == 0) firstUpper = true;
                    allLower = false;
                    foundUpperOrLower = true;
                }
                else if (Char.IsLower(template[i]))
                {
                    allUpper = false;
                    foundUpperOrLower = true;
                }
            }

            // change the case according to template
            if (foundUpperOrLower)
            {
                if (allLower)
                {
                    s = s.ToLowerInvariant();
                }
                else if (allUpper)
                {
                    s = s.ToUpperInvariant();
                }
                else if (firstUpper)
                {
                    if (!Char.IsUpper(s[0]))
                    {
                        s = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);
                    }
                }
            }

            return s;
        }

        #region Nested type: SuffixRule

        private class SuffixRule
        {
            private readonly string pluralSuffix;
            private readonly string singularSuffix;

            public SuffixRule(string singular, string plural)
            {
                singularSuffix = singular;
                pluralSuffix = plural;
            }

            public bool TryToPlural(string word, out string plural)
            {
                if (word.EndsWith(singularSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    plural = word.Substring(0, word.Length - singularSuffix.Length) + pluralSuffix;
                    return true;
                }
                plural = null;
                return false;
            }

            public bool TryToSingular(string word, out string singular)
            {
                if (word.EndsWith(pluralSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    singular = word.Substring(0, word.Length - pluralSuffix.Length) + singularSuffix;
                    return true;
                }
                singular = null;
                return false;
            }
        }

        #endregion

        #region Nested type: Word

        private class Word
        {
            public readonly string Plural;
            public readonly string Singular;

            public Word(string singular, string plural)
            {
                Singular = singular;
                Plural = plural;
            }
        }

        #endregion

        #endregion
    }
}