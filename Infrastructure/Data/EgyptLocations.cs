using Domain.Entities.AdEntities;

namespace Infrastructure.Data
{
    public static class EgyptLocations
    {
        public static readonly IReadOnlyList<Governorate> Governorates = new List<Governorate>
        {
            new() { Id = 1, NameAr = "القاهرة", NameEn = "Cairo", Cities = new()
            {
                new() { Id = 101, NameAr = "القاهرة", NameEn = "Cairo" },
                new() { Id = 102, NameAr = "مصر الجديدة", NameEn = "Heliopolis" },
                new() { Id = 103, NameAr = "مدينة نصر", NameEn = "NasrCity" },
                new() { Id = 104, NameAr = "المعادي", NameEn = "Maadi" },
                new() { Id = 105, NameAr = "الزمالك", NameEn = "Zamalek" },
                new() { Id = 106, NameAr = "شبرا", NameEn = "Shubra" },
                new() { Id = 107, NameAr = "القاهرة الجديدة", NameEn = "NewCairo" },
                new() { Id = 108, NameAr = "السادس من أكتوبر", NameEn = "SixthOfOctober" },
                new() { Id = 109, NameAr = "حلوان", NameEn = "Helwan" },
                new() { Id = 110, NameAr = "الشروق", NameEn = "Shorouk" },
                new() { Id = 111, NameAr = "بدر", NameEn = "Badr" },
                new() { Id = 112, NameAr = "العبور", NameEn = "Obour" },
                new() { Id = 113, NameAr = "عين شمس", NameEn = "AinShams" },
                new() { Id = 114, NameAr = "المرج", NameEn = "Marg" },
                new() { Id = 115, NameAr = "مدينة المستقبل", NameEn = "FutureCity" },
            }},
            new() { Id = 2, NameAr = "الإسكندرية", NameEn = "Alexandria", Cities = new()
            {
                new() { Id = 201, NameAr = "المنتزه", NameEn = "Montaza" },
                new() { Id = 202, NameAr = "سيدي جابر", NameEn = "SidiGaber" },
                new() { Id = 203, NameAr = "سموحة", NameEn = "Smouha" },
                new() { Id = 204, NameAr = "العجمي", NameEn = "Agami" },
                new() { Id = 205, NameAr = "برج العرب", NameEn = "BorgElArab" },
                new() { Id = 206, NameAr = "الدخيلة", NameEn = "Dekheila" },
                new() { Id = 207, NameAr = "المندرة", NameEn = "Mandara" },
                new() { Id = 208, NameAr = "جليم", NameEn = "Glim" },
                new() { Id = 209, NameAr = "ميامي", NameEn = "Miami" },
                new() { Id = 210, NameAr = "باب شرقي", NameEn = "BabSharqi" },
                new() { Id = 211, NameAr = "العامرية", NameEn = "Ameria" },
                new() { Id = 212, NameAr = "أبوقير", NameEn = "AbuQir" },
            }},
            new() { Id = 3, NameAr = "الجيزة", NameEn = "Giza", Cities = new()
            {
                new() { Id = 301, NameAr = "الدقي", NameEn = "Dokki" },
                new() { Id = 302, NameAr = "المهندسين", NameEn = "Mohandessin" },
                new() { Id = 303, NameAr = "الهرم", NameEn = "Haram" },
                new() { Id = 304, NameAr = "فيصل", NameEn = "Faisal" },
                new() { Id = 305, NameAr = "إمبابة", NameEn = "Imbaba" },
                new() { Id = 306, NameAr = "الشيخ زايد", NameEn = "SheikhZayed" },
                new() { Id = 307, NameAr = "أكتوبر", NameEn = "October" },
                new() { Id = 308, NameAr = "البدرشين", NameEn = "Badrashin" },
                new() { Id = 309, NameAr = "العياط", NameEn = "Ayat" },
                new() { Id = 310, NameAr = "الصف", NameEn = "Saf" },
                new() { Id = 311, NameAr = "أطفيح", NameEn = "Atfih" },
                new() { Id = 312, NameAr = "أوسيم", NameEn = "Ausim" },
                new() { Id = 313, NameAr = "كرداسة", NameEn = "Kerdasa" },
            }},
            new() { Id = 4, NameAr = "بورسعيد", NameEn = "PortSaid", Cities = new()
            {
                new() { Id = 401, NameAr = "بورفؤاد", NameEn = "PortFuad" },
                new() { Id = 402, NameAr = "المناخ", NameEn = "AlManakh" },
                new() { Id = 403, NameAr = "الزهور", NameEn = "AlZuhur" },
                new() { Id = 404, NameAr = "الشرق", NameEn = "AlSharq" },
                new() { Id = 405, NameAr = "الضواحي", NameEn = "AlDawahi" },
                new() { Id = 406, NameAr = "العرب", NameEn = "AlArab" },
                new() { Id = 407, NameAr = "الجنوب", NameEn = "AlJanoub" },
            }},
            new() { Id = 5, NameAr = "السويس", NameEn = "Suez", Cities = new()
            {
                new() { Id = 501, NameAr = "الأربعين", NameEn = "Arbaeen" },
                new() { Id = 502, NameAr = "عتاقة", NameEn = "Ataka" },
                new() { Id = 503, NameAr = "فيصل", NameEn = "FaisalSuez" },
                new() { Id = 504, NameAr = "الجناين", NameEn = "Ganayen" },
            }},
            new() { Id = 6, NameAr = "الأقصر", NameEn = "Luxor", Cities = new()
            {
                new() { Id = 601, NameAr = "الأقصر", NameEn = "LuxorCity" },
                new() { Id = 602, NameAr = "أرمنت", NameEn = "Armant" },
                new() { Id = 603, NameAr = "إسنا", NameEn = "Esna" },
                new() { Id = 604, NameAr = "طيبة الجديدة", NameEn = "NewTiba" },
                new() { Id = 605, NameAr = "البياضية", NameEn = "Bayadiya" },
            }},
            new() { Id = 7, NameAr = "أسوان", NameEn = "Aswan", Cities = new()
            {
                new() { Id = 701, NameAr = "أسوان", NameEn = "AswanCity" },
                new() { Id = 702, NameAr = "إدفو", NameEn = "Edfu" },
                new() { Id = 703, NameAr = "كوم أمبو", NameEn = "KomOmbo" },
                new() { Id = 704, NameAr = "دراو", NameEn = "Daraw" },
                new() { Id = 705, NameAr = "أبو سمبل", NameEn = "AbuSimbel" },
                new() { Id = 706, NameAr = "ناصر", NameEn = "Nasr" },
                new() { Id = 707, NameAr = "كلابشة", NameEn = "Kalabsha" },
            }},
            new() { Id = 8, NameAr = "أسيوط", NameEn = "Asyut", Cities = new()
            {
                new() { Id = 801, NameAr = "أسيوط", NameEn = "AsiutCity" },
                new() { Id = 802, NameAr = "أبنوب", NameEn = "Abnub" },
                new() { Id = 803, NameAr = "أبوتيج", NameEn = "AbuTig" },
                new() { Id = 804, NameAr = "ديروط", NameEn = "Dairut" },
                new() { Id = 805, NameAr = "منفلوط", NameEn = "Manfalut" },
                new() { Id = 806, NameAr = "ساحل سليم", NameEn = "SahelSelim" },
                new() { Id = 807, NameAr = "القوصية", NameEn = "Qusiya" },
                new() { Id = 808, NameAr = "البداري", NameEn = "Badari" },
                new() { Id = 809, NameAr = "أسيوط الجديدة", NameEn = "NewAsiut" },
            }},
            new() { Id = 9, NameAr = "بني سويف", NameEn = "BeniSuef", Cities = new()
            {
                new() { Id = 901, NameAr = "بني سويف", NameEn = "BeniSuefCity" },
                new() { Id = 902, NameAr = "ببا", NameEn = "Biba" },
                new() { Id = 903, NameAr = "الفشن", NameEn = "Fashn" },
                new() { Id = 904, NameAr = "ناصر", NameEn = "Nasser" },
                new() { Id = 905, NameAr = "سمسطا", NameEn = "Sumusta" },
                new() { Id = 906, NameAr = "إهناسيا المدينة", NameEn = "IhnasiaElMadina" },
                new() { Id = 907, NameAr = "الواسطى", NameEn = "Wasta" },
            }},
            new() { Id = 10, NameAr = "الفيوم", NameEn = "Faiyum", Cities = new()
            {
                new() { Id = 1001, NameAr = "الفيوم", NameEn = "FaiyumCity" },
                new() { Id = 1002, NameAr = "إبشواي", NameEn = "Ibsheway" },
                new() { Id = 1003, NameAr = "إطسا", NameEn = "Itsa" },
                new() { Id = 1004, NameAr = "سنورس", NameEn = "Sinnuris" },
                new() { Id = 1005, NameAr = "يوسف الصديق", NameEn = "YusufSediq" },
                new() { Id = 1006, NameAr = "طامية", NameEn = "Tamiya" },
            }},
            new() { Id = 11, NameAr = "الغربية", NameEn = "Gharbia", Cities = new()
            {
                new() { Id = 1101, NameAr = "طنطا", NameEn = "Tanta" },
                new() { Id = 1102, NameAr = "المحلة الكبرى", NameEn = "Mahalla" },
                new() { Id = 1103, NameAr = "كفر الزيات", NameEn = "KafrElZayat" },
                new() { Id = 1104, NameAr = "زفتى", NameEn = "Zefta" },
                new() { Id = 1105, NameAr = "سمنود", NameEn = "Samanoud" },
                new() { Id = 1106, NameAr = "بسيون", NameEn = "Basyoun" },
                new() { Id = 1107, NameAr = "قطور", NameEn = "Qutur" },
                new() { Id = 1108, NameAr = "السنطة", NameEn = "Santa" },
            }},
            new() { Id = 12, NameAr = "الإسماعيلية", NameEn = "Ismailia", Cities = new()
            {
                new() { Id = 1201, NameAr = "الإسماعيلية", NameEn = "IsmailiACity" },
                new() { Id = 1202, NameAr = "فايد", NameEn = "Fayed" },
                new() { Id = 1203, NameAr = "القنطرة شرق", NameEn = "QantaraEast" },
                new() { Id = 1204, NameAr = "القنطرة غرب", NameEn = "QantaraWest" },
                new() { Id = 1205, NameAr = "أبوعطوة", NameEn = "AbuAtewa" },
                new() { Id = 1206, NameAr = "القصاصين", NameEn = "Kasaseen" },
                new() { Id = 1207, NameAr = "التل الكبير", NameEn = "TallElKabir" },
            }},
            new() { Id = 13, NameAr = "المنوفية", NameEn = "Menofia", Cities = new()
            {
                new() { Id = 1301, NameAr = "شبين الكوم", NameEn = "ShebinElKom" },
                new() { Id = 1302, NameAr = "أشمون", NameEn = "Ashmoun" },
                new() { Id = 1303, NameAr = "منوف", NameEn = "Menouf" },
                new() { Id = 1304, NameAr = "بركة السبع", NameEn = "BirketElSab" },
                new() { Id = 1305, NameAr = "سرس الليان", NameEn = "SersElLian" },
                new() { Id = 1306, NameAr = "تلا", NameEn = "Tala" },
                new() { Id = 1307, NameAr = "قويسنا", NameEn = "Quesna" },
                new() { Id = 1308, NameAr = "مدينة السادات", NameEn = "Sadat" },
                new() { Id = 1309, NameAr = "الباجور", NameEn = "Bagour" },
            }},
            new() { Id = 14, NameAr = "المنيا", NameEn = "Minya", Cities = new()
            {
                new() { Id = 1401, NameAr = "المنيا", NameEn = "MinyaCity" },
                new() { Id = 1402, NameAr = "أبوقرقاص", NameEn = "AbuQurqas" },
                new() { Id = 1403, NameAr = "بني مزار", NameEn = "BeniMazar" },
                new() { Id = 1404, NameAr = "دير مواس", NameEn = "DeirMawas" },
                new() { Id = 1405, NameAr = "مغاغة", NameEn = "Maghagha" },
                new() { Id = 1406, NameAr = "ملوي", NameEn = "Mallawi" },
                new() { Id = 1407, NameAr = "مطاي", NameEn = "Matay" },
                new() { Id = 1408, NameAr = "سمالوط", NameEn = "Samalut" },
                new() { Id = 1409, NameAr = "المنيا الجديدة", NameEn = "NewMinya" },
            }},
            new() { Id = 15, NameAr = "القليوبية", NameEn = "Qalyubia", Cities = new()
            {
                new() { Id = 1501, NameAr = "بنها", NameEn = "Banha" },
                new() { Id = 1502, NameAr = "قليوب", NameEn = "Qalyub" },
                new() { Id = 1503, NameAr = "شبرا الخيمة", NameEn = "ShubralKheima" },
                new() { Id = 1504, NameAr = "الخانكة", NameEn = "Khanka" },
                new() { Id = 1505, NameAr = "قها", NameEn = "Qaha" },
                new() { Id = 1506, NameAr = "طوخ", NameEn = "Tukh" },
                new() { Id = 1507, NameAr = "كفر شكر", NameEn = "KafrShukr" },
                new() { Id = 1508, NameAr = "الخصوص", NameEn = "Khosous" },
            }},
            new() { Id = 16, NameAr = "الوادي الجديد", NameEn = "NewValley", Cities = new()
            {
                new() { Id = 1601, NameAr = "الخارجة", NameEn = "Kharga" },
                new() { Id = 1602, NameAr = "الداخلة", NameEn = "Dakhla" },
                new() { Id = 1603, NameAr = "الفرافرة", NameEn = "Farafra" },
                new() { Id = 1604, NameAr = "باريس", NameEn = "Baris" },
                new() { Id = 1605, NameAr = "بلاط", NameEn = "Balat" },
            }},
            new() { Id = 17, NameAr = "شمال سيناء", NameEn = "NorthSinai", Cities = new()
            {
                new() { Id = 1701, NameAr = "العريش", NameEn = "Arish" },
                new() { Id = 1702, NameAr = "الشيخ زويد", NameEn = "SheikhZuweid" },
                new() { Id = 1703, NameAr = "رفح", NameEn = "Rafah" },
                new() { Id = 1704, NameAr = "بئر العبد", NameEn = "BirElAbd" },
                new() { Id = 1705, NameAr = "حسنة", NameEn = "Hasna" },
                new() { Id = 1706, NameAr = "نخل", NameEn = "Nakhl" },
            }},
            new() { Id = 18, NameAr = "الشرقية", NameEn = "Sharqia", Cities = new()
            {
                new() { Id = 1801, NameAr = "الزقازيق", NameEn = "Zagazig" },
                new() { Id = 1802, NameAr = "بلبيس", NameEn = "Belbeis" },
                new() { Id = 1803, NameAr = "منيا القمح", NameEn = "MinyaElQamh" },
                new() { Id = 1804, NameAr = "أبوحماد", NameEn = "AbuHammad" },
                new() { Id = 1805, NameAr = "أبوكبير", NameEn = "AbuKabir" },
                new() { Id = 1806, NameAr = "فاقوس", NameEn = "Faqous" },
                new() { Id = 1807, NameAr = "ههيا", NameEn = "Hehia" },
                new() { Id = 1808, NameAr = "كفر صقر", NameEn = "KafrSaqr" },
                new() { Id = 1809, NameAr = "القنايات", NameEn = "Qenayet" },
                new() { Id = 1810, NameAr = "ديرب نجم", NameEn = "DiyarbNegm" },
                new() { Id = 1811, NameAr = "الصالحية الجديدة", NameEn = "SalhiyaElGedida" },
                new() { Id = 1812, NameAr = "الحسينية", NameEn = "Husseiniya" },
                new() { Id = 1813, NameAr = "العاشر من رمضان", NameEn = "TenthOfRamadan" },
            }},
            new() { Id = 19, NameAr = "سوهاج", NameEn = "Sohag", Cities = new()
            {
                new() { Id = 1901, NameAr = "سوهاج", NameEn = "SohagCity" },
                new() { Id = 1902, NameAr = "أخميم", NameEn = "Akhmim" },
                new() { Id = 1903, NameAr = "البلينا", NameEn = "Balyana" },
                new() { Id = 1904, NameAr = "دار السلام", NameEn = "DarElSalam" },
                new() { Id = 1905, NameAr = "جرجا", NameEn = "Gerga" },
                new() { Id = 1906, NameAr = "جهينة", NameEn = "Juhayna" },
                new() { Id = 1907, NameAr = "المراغة", NameEn = "Maragha" },
                new() { Id = 1908, NameAr = "ساقلتة", NameEn = "Saqultah" },
                new() { Id = 1909, NameAr = "طهطا", NameEn = "Tahta" },
                new() { Id = 1910, NameAr = "تيما", NameEn = "Tima" },
            }},
            new() { Id = 20, NameAr = "جنوب سيناء", NameEn = "SouthSinai", Cities = new()
            {
                new() { Id = 2001, NameAr = "شرم الشيخ", NameEn = "SharmElSheikh" },
                new() { Id = 2002, NameAr = "دهب", NameEn = "Dahab" },
                new() { Id = 2003, NameAr = "نويبع", NameEn = "Nuweiba" },
                new() { Id = 2004, NameAr = "طابا", NameEn = "Taba" },
                new() { Id = 2005, NameAr = "رأس سدر", NameEn = "RasSidr" },
                new() { Id = 2006, NameAr = "سانت كاترين", NameEn = "SaintCatherine" },
                new() { Id = 2007, NameAr = "أبورديس", NameEn = "AbuRudeis" },
                new() { Id = 2008, NameAr = "طور سيناء", NameEn = "TorSinai" },
            }},
            new() { Id = 21, NameAr = "كفر الشيخ", NameEn = "KafrElSheikh", Cities = new()
            {
                new() { Id = 2101, NameAr = "كفر الشيخ", NameEn = "KafrElSheikhCity" },
                new() { Id = 2102, NameAr = "بلطيم", NameEn = "Baltim" },
                new() { Id = 2103, NameAr = "بيلا", NameEn = "Biyala" },
                new() { Id = 2104, NameAr = "دسوق", NameEn = "Desouk" },
                new() { Id = 2105, NameAr = "فوه", NameEn = "Fuwwa" },
                new() { Id = 2106, NameAr = "مطوبس", NameEn = "Metoubes" },
                new() { Id = 2107, NameAr = "قلين", NameEn = "Qallin" },
                new() { Id = 2108, NameAr = "سيدي سالم", NameEn = "SidiSalem" },
                new() { Id = 2109, NameAr = "الحامول", NameEn = "Hambat" },
            }},
            new() { Id = 22, NameAr = "مطروح", NameEn = "Matrouh", Cities = new()
            {
                new() { Id = 2201, NameAr = "مرسى مطروح", NameEn = "MarsaMatruh" },
                new() { Id = 2202, NameAr = "سيدي براني", NameEn = "SidiBarrani" },
                new() { Id = 2203, NameAr = "سيوة", NameEn = "Siwa" },
                new() { Id = 2204, NameAr = "السلوم", NameEn = "Salloum" },
                new() { Id = 2205, NameAr = "الضبعة", NameEn = "Daba" },
                new() { Id = 2206, NameAr = "العلمين", NameEn = "Alamein" },
                new() { Id = 2207, NameAr = "النجيلة", NameEn = "Negila" },
            }},
            new() { Id = 23, NameAr = "قنا", NameEn = "Qena", Cities = new()
            {
                new() { Id = 2301, NameAr = "قنا", NameEn = "QenaCity" },
                new() { Id = 2302, NameAr = "أبوتشت", NameEn = "AbuTesht" },
                new() { Id = 2303, NameAr = "دشنا", NameEn = "Deshna" },
                new() { Id = 2304, NameAr = "فرشوط", NameEn = "Farshut" },
                new() { Id = 2305, NameAr = "نقادة", NameEn = "Naqada" },
                new() { Id = 2306, NameAr = "نجع حمادي", NameEn = "NagHammadi" },
                new() { Id = 2307, NameAr = "قفط", NameEn = "Quft" },
                new() { Id = 2308, NameAr = "قوص", NameEn = "Qus" },
            }},
            new() { Id = 24, NameAr = "دمياط", NameEn = "Damietta", Cities = new()
            {
                new() { Id = 2401, NameAr = "دمياط", NameEn = "DamiettaCity" },
                new() { Id = 2402, NameAr = "فارسكور", NameEn = "Faraskur" },
                new() { Id = 2403, NameAr = "كفر سعد", NameEn = "KafrSaad" },
                new() { Id = 2404, NameAr = "الزرقا", NameEn = "Zarqa" },
                new() { Id = 2405, NameAr = "دمياط الجديدة", NameEn = "NewDamietta" },
                new() { Id = 2406, NameAr = "رأس البر", NameEn = "RasElBar" },
            }},
            new() { Id = 25, NameAr = "الدقهلية", NameEn = "Dakahlia", Cities = new()
            {
                new() { Id = 2501, NameAr = "المنصورة", NameEn = "Mansoura" },
                new() { Id = 2502, NameAr = "طلخا", NameEn = "Talkha" },
                new() { Id = 2503, NameAr = "أجا", NameEn = "Aga" },
                new() { Id = 2504, NameAr = "بلقاس", NameEn = "Belqas" },
                new() { Id = 2505, NameAr = "دكرنس", NameEn = "Dekernes" },
                new() { Id = 2506, NameAr = "جمصة", NameEn = "Gamasa" },
                new() { Id = 2507, NameAr = "المنزلة", NameEn = "Manzala" },
                new() { Id = 2508, NameAr = "ميت غمر", NameEn = "MeetGhamr" },
                new() { Id = 2509, NameAr = "شربين", NameEn = "Sherbeen" },
                new() { Id = 2510, NameAr = "سنبلاوين", NameEn = "Sinbillawein" },
                new() { Id = 2511, NameAr = "المطرية", NameEn = "Matariya" },
                new() { Id = 2512, NameAr = "منية النصر", NameEn = "MinyatElNasr" },
            }},
            new() { Id = 26, NameAr = "البحر الأحمر", NameEn = "RedSea", Cities = new()
            {
                new() { Id = 2601, NameAr = "الغردقة", NameEn = "Hurghada" },
                new() { Id = 2602, NameAr = "سفاجا", NameEn = "Safaga" },
                new() { Id = 2603, NameAr = "القصير", NameEn = "Quseer" },
                new() { Id = 2604, NameAr = "مرسى علم", NameEn = "MarsaAlam" },
                new() { Id = 2605, NameAr = "شلاتين", NameEn = "Shalateen" },
                new() { Id = 2606, NameAr = "حلايب", NameEn = "Halaib" },
                new() { Id = 2607, NameAr = "رأس غارب", NameEn = "RasGharib" },
            }},
            new() { Id = 27, NameAr = "البحيرة", NameEn = "Beheira", Cities = new()
            {
                new() { Id = 2701, NameAr = "دمنهور", NameEn = "Damanhur" },
                new() { Id = 2702, NameAr = "كفر الدوار", NameEn = "KafrElDawwar" },
                new() { Id = 2703, NameAr = "رشيد", NameEn = "Rashid" },
                new() { Id = 2704, NameAr = "أبوحمص", NameEn = "AbuHomos" },
                new() { Id = 2705, NameAr = "أبومطامير", NameEn = "AbuMatamir" },
                new() { Id = 2706, NameAr = "دلنجات", NameEn = "Delengat" },
                new() { Id = 2707, NameAr = "إدكو", NameEn = "Edku" },
                new() { Id = 2708, NameAr = "حوش عيسى", NameEn = "HoshIssa" },
                new() { Id = 2709, NameAr = "إيتاي البارود", NameEn = "ItayElBaroud" },
                new() { Id = 2710, NameAr = "كوم حمادة", NameEn = "KomHamada" },
                new() { Id = 2711, NameAr = "المحمودية", NameEn = "Mahmoudiya" },
                new() { Id = 2712, NameAr = "النوبارية", NameEn = "Nubaria" },
                new() { Id = 2713, NameAr = "الرحمانية", NameEn = "Rahmaniya" },
                new() { Id = 2714, NameAr = "شبراخيت", NameEn = "Shubrakhit" },
                new() { Id = 2715, NameAr = "وادي النطرون", NameEn = "WadiElNatrun" },
            }},
        }.AsReadOnly();

        // Dictionaries for O(1) lookups instead of O(n) FirstOrDefault
        private static readonly Dictionary<int, Governorate> _governorateMap =
            Governorates.ToDictionary(g => g.Id);

        private static readonly Dictionary<(int, int), City> _cityMap =
            Governorates.SelectMany(g => g.Cities.Select(c => (g.Id, c)))
                        .ToDictionary(x => (x.Id, x.c.Id), x => x.c);

        public static Governorate? GetGovernorate(int id) =>
            _governorateMap.GetValueOrDefault(id);

        public static City? GetCity(int governorateId, int cityId) =>
            _cityMap.GetValueOrDefault((governorateId, cityId));

        public static bool CityBelongsToGovernorate(int governorateId, int cityId) =>
            _cityMap.ContainsKey((governorateId, cityId));
    }
}