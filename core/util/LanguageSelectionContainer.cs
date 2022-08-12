// ****************************************************************************
// 
// Copyright (C) 2005-2018 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace MeGUI
{
	/// <summary>
	/// A container for the selectable languages in MeGUI
	/// </summary>
	public class LanguageSelectionContainer
	{
        // used by all tools except MP4box
        private static readonly Dictionary<string, string> languagesBibliographic; 
        private static readonly Dictionary<string, string> languagesReverseBibliographic;

        // used by MP4box
        private static readonly Dictionary<string, string> languagesTerminology;
        private static readonly Dictionary<string, string> languagesReverseTerminology;

        private static readonly Dictionary<string, string> languagesISO2;
        private static readonly Dictionary<string, string> languagesReverseISO2;

        /// <summary>
        /// uses the ISO 639-2/B language codes
        /// </summary>
        public static Dictionary<string, string> Languages
        {
            get
            {
                return languagesBibliographic;
            }
        }

        /// <summary>
        /// uses the ISO 639-2/T language codes
        /// </summary>
        public static Dictionary<string, string> LanguagesTerminology
        {
            get
            {
                return languagesTerminology;
            }
        }

        private static void addLanguage(string name, string iso3B, string iso3T, string iso2)
        {
            languagesBibliographic.Add(name, iso3B);
            languagesReverseBibliographic.Add(iso3B, name);

            if (String.IsNullOrEmpty(iso3T))
            {
                languagesTerminology.Add(name, iso3B);
                languagesReverseTerminology.Add(iso3B, name);
            }
            else
            {
                languagesTerminology.Add(name, iso3T);
                languagesReverseTerminology.Add(iso3T, name);
            }

            if (!String.IsNullOrEmpty(iso2))
            {
                languagesISO2.Add(name, iso2);
                languagesReverseISO2.Add(iso2, name);
            }
        }

        static LanguageSelectionContainer()
        {
            // http://www.loc.gov/standards/iso639-2/php/code_list.php
            // https://en.wikipedia.org/wiki/List_of_ISO_639-2_codes
            // Attention: check all tools (eac3to, mkvmerge, mediainfo, ...)

            languagesBibliographic = new Dictionary<string, string>();
            languagesReverseBibliographic = new Dictionary<string, string>();

            languagesTerminology = new Dictionary<string, string>();
            languagesReverseTerminology = new Dictionary<string, string>();

            languagesISO2 = new Dictionary<string, string>();
            languagesReverseISO2 = new Dictionary<string, string>();

            addLanguage("Abkhazian", "abk", "", "ab");
            addLanguage("Achinese", "ace", "", "");
            addLanguage("Acoli", "ach", "", "");
            addLanguage("Adangme", "ada", "", "");
            addLanguage("Adyghe", "ady", "", "");
            addLanguage("Afar", "aar", "", "aa");
            addLanguage("Afrikaans", "afr", "", "af");
            addLanguage("Ainu", "ain", "", "");
            addLanguage("Akan", "aka", "", "ak");
            addLanguage("Albanian", "alb", "sqi", "sq");
            addLanguage("Aleut", "ale", "", "");
            addLanguage("Amharic", "amh", "", "am");
            addLanguage("Angika", "anp", "", "");
            addLanguage("Arabic", "ara", "", "ar");
            addLanguage("Aragonese", "arg", "", "an");
            addLanguage("Arapaho", "arp", "", "");
            addLanguage("Arawak", "arw", "", "");
            addLanguage("Armenian", "arm", "hye", "hy");
            addLanguage("Aromanian", "rup", "", "");
            addLanguage("Assamese", "asm", "", "as");
            addLanguage("Asturian", "ast", "", "");
            addLanguage("Avaric", "ava", "", "av");
            addLanguage("Awadhi", "awa", "", "");
            addLanguage("Aymara", "aym", "", "ay");
            addLanguage("Azerbaijani", "aze", "", "az");
            addLanguage("Balinese", "ban", "", "");
            addLanguage("Baluchi", "bal", "", "");
            addLanguage("Bambara", "bam", "", "bm");
            addLanguage("Basa", "bas", "", "");
            addLanguage("Bashkir", "bak", "", "ba");
            addLanguage("Basque", "baq", "eus", "eu");
            addLanguage("Beja", "bej", "", "");
            addLanguage("Belarusian", "bel", "", "be");
            addLanguage("Bemba", "bem", "", "");
            addLanguage("Bengali", "ben", "", "bn");
            addLanguage("Bhojpuri", "bho", "", "");
            addLanguage("Bikol", "bik", "", "");
            addLanguage("Bini", "bin", "", "");
            addLanguage("Bislama", "bis", "", "bi");
            addLanguage("Blin", "byn", "", "");
            addLanguage("Bosnian", "bos", "", "bs");
            addLanguage("Braj", "bra", "", "");
            addLanguage("Breton", "bre", "", "br");
            addLanguage("Buginese", "bug", "", "");
            addLanguage("Bulgarian", "bul", "", "bg");
            addLanguage("Buriat", "bua", "", "");
            addLanguage("Burmese", "bur", "mya", "my");
            addLanguage("Caddo", "cad", "", "");
            addLanguage("Catalan", "cat", "", "ca");
            addLanguage("Cebuano", "ceb", "", "");
            addLanguage("Central Khmer", "khm", "", "km");
            addLanguage("Chamorro", "cha", "", "ch");
            addLanguage("Chechen", "che", "", "ce");
            addLanguage("Cherokee", "chr", "", "");
            addLanguage("Cheyenne", "chy", "", "");
            addLanguage("Chichewa", "nya", "", "ny");
            addLanguage("Chinese", "chi", "zho", "zh");
            addLanguage("Chinook jargon", "chn", "", "");
            addLanguage("Chipewyan", "chp", "", "");
            addLanguage("Choctaw", "cho", "", "");
            addLanguage("Chuukese", "chk", "", "");
            addLanguage("Chuvash", "chv", "", "cv");
            addLanguage("Cornish", "cor", "", "kw");
            addLanguage("Corsican", "cos", "", "co");
            addLanguage("Cree", "cre", "", "cr");
            addLanguage("Creek", "mus", "", "");
            addLanguage("Crimean Tatar", "crh", "", "");
            addLanguage("Croatian", "hrv", "", "hr");
            addLanguage("Czech", "cze", "ces", "cs");
            addLanguage("Dakota", "dak", "", "");
            addLanguage("Danish", "dan", "", "da");
            addLanguage("Dargwa", "dar", "", "");
            addLanguage("Delaware", "del", "", "");
            addLanguage("Dinka", "din", "", "");
            addLanguage("Divehi", "div", "", "dv");
            addLanguage("Dogri", "doi", "", "");
            addLanguage("Dogrib", "dgr", "", "");
            addLanguage("Duala", "dua", "", "");
            addLanguage("Dutch", "dut", "nld", "nl");
            addLanguage("Dyula", "dyu", "", "");
            addLanguage("Dzongkha", "dzo", "", "dz");
            addLanguage("Eastern Frisian", "frs", "", "");
            addLanguage("Efik", "efi", "", "");
            addLanguage("Ekajuk", "eka", "", "");
            addLanguage("English", "eng", "", "en");
            addLanguage("Erzya", "myv", "", "");
            addLanguage("Estonian", "est", "", "et");
            addLanguage("Ewe", "ewe", "", "ee");
            addLanguage("Ewondo", "ewo", "", "");
            addLanguage("Fang", "fan", "", "");
            addLanguage("Fanti", "fat", "", "");
            addLanguage("Faroese", "fao", "", "fo");
            addLanguage("Fijian", "fij", "", "fj");
            addLanguage("Filipino", "fil", "", "");
            addLanguage("Finnish", "fin", "", "fi");
            addLanguage("Fon", "fon", "", "");
            addLanguage("French", "fre", "fra", "fr");
            addLanguage("Friulian", "fur", "", "");
            addLanguage("Fulah", "ful", "", "ff");
            addLanguage("Ga", "gaa", "", "");
            addLanguage("Gaelic", "gla", "", "gd");
            addLanguage("Galibi Carib", "car", "", "");
            addLanguage("Galician", "glg", "", "gl");
            addLanguage("Ganda", "lug", "", "lg");
            addLanguage("Gayo", "gay", "", "");
            addLanguage("Gbaya", "gba", "", "");
            addLanguage("Georgian", "geo", "kat", "ka");
            addLanguage("German", "ger", "deu", "de");
            addLanguage("Gilbertese", "gil", "", "");
            addLanguage("Gondi", "gon", "", "");
            addLanguage("Gorontalo", "gor", "", "");
            addLanguage("Grebo", "grb", "", "");
            addLanguage("Greek", "gre", "ell", "el");
            addLanguage("Guarani", "grn", "", "gn");
            addLanguage("Gujarati", "guj", "", "gu");
            addLanguage("Gwich'in", "gwi", "", "");
            addLanguage("Haida", "hai", "", "");
            addLanguage("Haitian", "hat", "", "ht");
            addLanguage("Hausa", "hau", "", "ha");
            addLanguage("Hawaiian", "haw", "", "");
            addLanguage("Hebrew", "heb", "", "he");
            addLanguage("Herero", "her", "", "hz");
            addLanguage("Hiligaynon", "hil", "", "");
            addLanguage("Hindi", "hin", "", "hi");
            addLanguage("Hiri Motu", "hmo", "", "ho");
            addLanguage("Hmong", "hmn", "", "");
            addLanguage("Hungarian", "hun", "", "hu");
            addLanguage("Hupa", "hup", "", "");
            addLanguage("Iban", "iba", "", "");
            addLanguage("Icelandic", "ice", "isl", "is");
            addLanguage("Igbo", "ibo", "", "ig");
            addLanguage("Iloko", "ilo", "", "");
            addLanguage("Inari Sami", "smn", "", "");
            addLanguage("Indonesian", "ind", "", "id");
            addLanguage("Ingush", "inh", "", "");
            addLanguage("Inuktitut", "iku", "", "iu");
            addLanguage("Inupiaq", "ipk", "", "ik");
            addLanguage("Irish", "gle", "", "ga");
            addLanguage("Italian", "ita", "", "it");
            addLanguage("Japanese", "jpn", "", "ja");
            addLanguage("Javanese", "jav", "", "jv");
            addLanguage("Judeo-Arabic", "jrb", "", "");
            addLanguage("Judeo-Persian", "jpr", "", "");
            addLanguage("Kabardian", "kbd", "", "");
            addLanguage("Kabyle", "kab", "", "");
            addLanguage("Kachin", "kac", "", "");
            addLanguage("Kalaallisut", "kal", "", "kl");
            addLanguage("Kalmyk", "xal", "", "");
            addLanguage("Kamba", "kam", "", "");
            addLanguage("Kannada", "kan", "", "kn");
            addLanguage("Kanuri", "kau", "", "kr");
            addLanguage("Karachay-Balkar", "krc", "", "");
            addLanguage("Kara-Kalpak", "kaa", "", "");
            addLanguage("Karelian", "krl", "", "");
            addLanguage("Kashmiri", "kas", "", "ks");
            addLanguage("Kashubian", "csb", "", "");
            addLanguage("Kazakh", "kaz", "", "kk");
            addLanguage("Khasi", "kha", "", "");
            addLanguage("Kikuyu", "kik", "", "ki");
            addLanguage("Kimbundu", "kmb", "", "");
            addLanguage("Kinyarwanda", "kin", "", "rw");
            addLanguage("Kirghiz", "kir", "", "ky");
            addLanguage("Komi", "kom", "", "kv");
            addLanguage("Kongo", "kon", "", "kg");
            addLanguage("Konkani", "kok", "", "");
            addLanguage("Korean", "kor", "", "ko");
            addLanguage("Kosraean", "kos", "", "");
            addLanguage("Kpelle", "kpe", "", "");
            addLanguage("Kuanyama", "kua", "", "kj");
            addLanguage("Kumyk", "kum", "", "");
            addLanguage("Kurdish", "kur", "", "ku");
            addLanguage("Kurukh", "kru", "", "");
            addLanguage("Kutenai", "kut", "", "");
            addLanguage("Ladino", "lad", "", "");
            addLanguage("Lahnda", "lah", "", "");
            addLanguage("Lamba", "lam", "", "");
            addLanguage("Lao", "lao", "", "lo");
            addLanguage("Latvian", "lav", "", "lv");
            addLanguage("Lezghian", "lez", "", "");
            addLanguage("Limburgan", "lim", "", "li");
            addLanguage("Lingala", "lin", "", "ln");
            addLanguage("Lithuanian", "lit", "", "lt");
            addLanguage("Low German", "nds", "", "");
            addLanguage("Lower Sorbian", "dsb", "", "");
            addLanguage("Lozi", "loz", "", "");
            addLanguage("Luba-Katanga", "lub", "", "lu");
            addLanguage("Luba-Lulua", "lua", "", "");
            addLanguage("Luiseno", "lui", "", "");
            addLanguage("Lule Sami", "smj", "", "");
            addLanguage("Lunda", "lun", "", "");
            addLanguage("Luo", "luo", "", "");
            addLanguage("Lushai", "lus", "", "");
            addLanguage("Luxembourgish", "ltz", "", "lb");
            addLanguage("Macedonian", "mac", "mkd", "mk");
            addLanguage("Madurese", "mad", "", "");
            addLanguage("Magahi", "mag", "", "");
            addLanguage("Maithili", "mai", "", "");
            addLanguage("Makasar", "mak", "", "");
            addLanguage("Malagasy", "mlg", "", "mg");
            addLanguage("Malay", "may", "msa", "ms");
            addLanguage("Malayalam", "mal", "", "ml");
            addLanguage("Maltese", "mlt", "", "mt");
            addLanguage("Manchu", "mnc", "", "");
            addLanguage("Mandar", "mdr", "", "");
            addLanguage("Mandingo", "man", "", "");
            addLanguage("Manipuri", "mni", "", "");
            addLanguage("Manx", "glv", "", "gv");
            addLanguage("Maori", "mao", "mri", "mi");
            addLanguage("Mapudungun", "arn", "", "");
            addLanguage("Marathi", "mar", "", "mr");
            addLanguage("Mari", "chm", "", "");
            addLanguage("Marshallese", "mah", "", "mh");
            addLanguage("Marwari", "mwr", "", "");
            addLanguage("Masai", "mas", "", "");
            addLanguage("Mende", "men", "", "");
            addLanguage("Mi'kmaq", "mic", "", "");
            addLanguage("Minangkabau", "min", "", "");
            addLanguage("Mirandese", "mwl", "", "");
            addLanguage("Mohawk", "moh", "", "");
            addLanguage("Moksha", "mdf", "", "");
            addLanguage("Moldavian", "mol", "", "mo");
            addLanguage("Mongo", "lol", "", "");
            addLanguage("Mongolian", "mon", "", "mn");
            addLanguage("Mossi", "mos", "", "");
            addLanguage("Nauru", "nau", "", "na");
            addLanguage("Navajo", "nav", "", "nv");
            addLanguage("Ndebele, North", "nde", "", "nd");
            addLanguage("Ndebele, South", "nbl", "", "nr");
            addLanguage("Ndonga", "ndo", "", "ng");
            addLanguage("Neapolitan", "nap", "", "");
            addLanguage("Nepal Bhasa", "new", "", "");
            addLanguage("Nepali", "nep", "", "ne");
            addLanguage("Nias", "nia", "", "");
            addLanguage("Niuean", "niu", "", "");
            addLanguage("N'Ko", "nqo", "", "");
            addLanguage("Nogai", "nog", "", "");
            addLanguage("Northern Frisian", "frr", "", "");
            addLanguage("Northern Sami", "sme", "", "se");
            addLanguage("Norwegian", "nor", "", "no");
            addLanguage("Norwegian Bokmål", "nob", "", "nb");
            addLanguage("Norwegian Nynorsk", "nno", "", "nn");
            addLanguage("Nyamwezi", "nym", "", "");
            addLanguage("Nyankole", "nyn", "", "");
            addLanguage("Nyoro", "nyo", "", "");
            addLanguage("Nzima", "nzi", "", "");
            addLanguage("Occitan", "oci", "", "oc");
            addLanguage("Ojibwa", "oji", "", "oj");
            addLanguage("Oriya", "ori", "", "or");
            addLanguage("Oromo", "orm", "", "om");
            addLanguage("Osage", "osa", "", "");
            addLanguage("Ossetian", "oss", "", "os");
            addLanguage("Palauan", "pau", "", "");
            addLanguage("Pampanga", "pam", "", "");
            addLanguage("Pangasinan", "pag", "", "");
            addLanguage("Panjabi", "pan", "", "pa");
            addLanguage("Papiamento", "pap", "", "");
            addLanguage("Pedi", "nso", "", "");
            addLanguage("Persian", "per", "fas", "fa");
            addLanguage("Pohnpeian", "pon", "", "");
            addLanguage("Polish", "pol", "", "pl");
            addLanguage("Portuguese", "por", "", "pt");
            addLanguage("Pushto", "pus", "", "ps");
            addLanguage("Quechua", "que", "", "qu");
            addLanguage("Rajasthani", "raj", "", "");
            addLanguage("Rapanui", "rap", "", "");
            addLanguage("Rarotongan", "rar", "", "");
            addLanguage("Reserved for local use", "qaa", "", "");
            addLanguage("Romanian", "rum", "ron", "ro");
            addLanguage("Romansh", "roh", "", "rm");
            addLanguage("Romany", "rom", "", "");
            addLanguage("Rundi", "run", "", "rn");
            addLanguage("Russian", "rus", "", "ru");
            addLanguage("Samoan", "smo", "", "sm");
            addLanguage("Sandawe", "sad", "", "");
            addLanguage("Sango", "sag", "", "sg");
            addLanguage("Santali", "sat", "", "");
            addLanguage("Sardinian", "srd", "", "sc");
            addLanguage("Sasak", "sas", "", "");
            addLanguage("Scots", "sco", "", "");
            addLanguage("Selkup", "sel", "", "");
            addLanguage("Serbian", "srp", "", "sr");
            addLanguage("Serer", "srr", "", "");
            addLanguage("Shan", "shn", "", "");
            addLanguage("Shona", "sna", "", "sn");
            addLanguage("Sichuan Yi", "iii", "", "ii");
            addLanguage("Sicilian", "scn", "", "");
            addLanguage("Sidamo", "sid", "", "");
            addLanguage("Siksika", "bla", "", "");
            addLanguage("Sindhi", "snd", "", "sd");
            addLanguage("Sinhala", "sin", "", "si");
            addLanguage("Skolt Sami", "sms", "", "");
            addLanguage("Slave (Athapascan)", "den", "", "");
            addLanguage("Slovak", "slo", "slk", "sk");
            addLanguage("Slovenian", "slv", "", "sl");
            addLanguage("Somali", "som", "", "so");
            addLanguage("Soninke", "snk", "", "");
            addLanguage("Sotho, Southern", "sot", "", "st");
            addLanguage("Southern Altai", "alt", "", "");
            addLanguage("Southern Sami", "sma", "", "");
            addLanguage("Spanish", "spa", "", "es");
            addLanguage("Sranan Tongo", "srn", "", "");
            addLanguage("Standard Moroccan Tamazight", "zgh", "", "");
            addLanguage("Sukuma", "suk", "", "");
            addLanguage("Sundanese", "sun", "", "su");
            addLanguage("Susu", "sus", "", "");
            addLanguage("Swahili", "swa", "", "sw");
            addLanguage("Swati", "ssw", "", "ss");
            addLanguage("Swedish", "swe", "", "sv");
            addLanguage("Swiss German", "gsw", "", "");
            addLanguage("Syriac", "syr", "", "");
            addLanguage("Tagalog", "tgl", "", "tl");
            addLanguage("Tahitian", "tah", "", "ty");
            addLanguage("Tajik", "tgk", "", "tg");
            addLanguage("Tamashek", "tmh", "", "");
            addLanguage("Tamil", "tam", "", "ta");
            addLanguage("Tatar", "tat", "", "tt");
            addLanguage("Telugu", "tel", "", "te");
            addLanguage("Tereno", "ter", "", "");
            addLanguage("Tetum", "tet", "", "");
            addLanguage("Thai", "tha", "", "th");
            addLanguage("Tibetan", "tib", "bod", "bo");
            addLanguage("Tigre", "tig", "", "");
            addLanguage("Tigrinya", "tir", "", "ti");
            addLanguage("Timne", "tem", "", "");
            addLanguage("Tiv", "tiv", "", "");
            addLanguage("Tlingit", "tli", "", "");
            addLanguage("Tok Pisin", "tpi", "", "");
            addLanguage("Tokelau", "tkl", "", "");
            addLanguage("Tonga (Nyasa)", "tog", "", "");
            addLanguage("Tonga (Tonga Islands)", "ton", "", "to");
            addLanguage("Tsimshian", "tsi", "", "");
            addLanguage("Tsonga", "tso", "", "ts");
            addLanguage("Tswana", "tsn", "", "tn");
            addLanguage("Tumbuka", "tum", "", "");
            addLanguage("Turkish", "tur", "", "tr");
            addLanguage("Turkmen", "tuk", "", "tk");
            addLanguage("Tuvalu", "tvl", "", "");
            addLanguage("Tuvinian", "tyv", "", "");
            addLanguage("Twi", "twi", "", "tw");
            addLanguage("Udmurt", "udm", "", "");
            addLanguage("Uighur", "uig", "", "ug");
            addLanguage("Ukrainian", "ukr", "", "uk");
            addLanguage("Umbundu", "umb", "", "");
            addLanguage("Uncoded languages", "mis", "", "");
            addLanguage("Undetermined", "und", "", "");
            addLanguage("Upper Sorbian", "hsb", "", "");
            addLanguage("Urdu", "urd", "", "ur");
            addLanguage("Uzbek", "uzb", "", "uz");
            addLanguage("Vai", "vai", "", "");
            addLanguage("Venda", "ven", "", "ve");
            addLanguage("Vietnamese", "vie", "", "vi");
            addLanguage("Votic", "vot", "", "");
            addLanguage("Walloon", "wln", "", "wa");
            addLanguage("Waray", "war", "", "");
            addLanguage("Washo", "was", "", "");
            addLanguage("Welsh", "wel", "cym", "cy");
            addLanguage("Western Frisian", "fry", "", "fy");
            addLanguage("Wolaitta", "wal", "", "");
            addLanguage("Wolof", "wol", "", "wo");
            addLanguage("Xhosa", "xho", "", "xh");
            addLanguage("Yakut", "sah", "", "");
            addLanguage("Yao", "yao", "", "");
            addLanguage("Yapese", "yap", "", "");
            addLanguage("Yiddish", "yid", "", "yi");
            addLanguage("Yoruba", "yor", "", "yo");
            addLanguage("Zapotec", "zap", "", "");
            addLanguage("Zaza", "zza", "", "");
            addLanguage("Zenaga", "zen", "", "");
            addLanguage("Zhuang", "zha", "", "za");
            addLanguage("Zulu", "zul", "", "zu");
            addLanguage("Zuni", "zun", "", "");
        }

        ///<summary>
        ///Convert the 2 or 3 char string to the full language name
        ///</summary>
        public static string LookupISOCode(string code)
		{
            if (code.Length == 2)
            {
                if (languagesReverseISO2.ContainsKey(code))
                    return languagesReverseISO2[code];    
            }
            else if (code.Length == 3)
            {
                if (languagesReverseBibliographic.ContainsKey(code))
                    return languagesReverseBibliographic[code];
                else if (languagesReverseTerminology.ContainsKey(code))
                    return languagesReverseTerminology[code];
            }
            return "";
		}

        public static bool IsLanguageAvailable(string language)
        {
            if (languagesBibliographic.ContainsKey(language))
                return true;
            return false;
        }

        public static string GetLanguageFromFileName(string strFileName)
        {
            ArrayList arrText = new ArrayList();
            string strText = string.Empty;
            foreach (char c in strFileName.ToLowerInvariant())
            {
                if (!char.IsLetter(c))
                {
                    if (!String.IsNullOrEmpty(strText))
                        arrText.Add(strText);
                    strText = string.Empty;
                }
                else
                    strText += c;
            }
            if (!String.IsNullOrEmpty(strText))
                arrText.Add(strText);
            arrText.Reverse();

            foreach (string strTextPart in arrText)
            {
                foreach (KeyValuePair<string, string> strLanguage in languagesBibliographic)
                {
                    if (strTextPart.Equals(strLanguage.Key.ToLowerInvariant()))
                        return strLanguage.Key;
                }
            }

            foreach (string strTextPart in arrText)
            {
                foreach (KeyValuePair<string, string> strLanguage in languagesBibliographic)
                {
                    if (strTextPart.Equals(strLanguage.Value.ToLowerInvariant()))
                        return strLanguage.Key;
                }
            }

            foreach (string strTextPart in arrText)
            {
                foreach (KeyValuePair<string, string> strLanguage in languagesTerminology)
                {
                    if (strTextPart.Equals(strLanguage.Value.ToLowerInvariant()))
                        return strLanguage.Key;
                }
            }

            return string.Empty;
        }
	}
}