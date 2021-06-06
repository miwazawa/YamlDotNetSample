using System;
using System.IO;
using System.Text;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace YamlDotNetSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string yamlPath = @"C:\DQ\param.yaml";
            string savePath = @"C:\DQ\param.yaml";

            // Serialize
            //writeYamlUsingObject(savePath);
            //writeYamlUsingStreamV1(savePath);
            //writeYamlUsingStreamV2(savePath);

            // Deserialize
            //readYamlUsingObject(yamlPath);
            //readYamlUsingStream(yamlPath);

        }

        /// <summary>
        /// オブジェクトを使ってYamlファイルをシリアライズする関数
        /// </summary>
        /// <param name="yamlPath">String型：Ymalファイル出力パス</param>
        private static void writeYamlUsingObject(string savePath)
        {
            var YamlObj = new YamlObj
            {
                Version = "1.0.0",
                Date = new DateTime(1988, 2, 10),
                PersonInfo = new PersonInfo
                {
                    HP = 20,
                    MP = 10,
                    Attack = 4,
                    Defence = 5,
                    Speed = 2,
                    Luck = 3
                },
                Classes = new Classes[]
                {
                    new Classes
                    {
                        Name = "せんし",
                        Strategy = "ガンガンいこうぜ",
                        Feature = "力持ち"
                    },
                    new Classes
                    {
                        Name = "そうりょ",
                        Strategy = "いのちだいじに",
                        Feature = "癒し系"
                    },
                    new Classes
                    {
                        Name = "まほうつかい",
                        Strategy = "じゅもんつかうな",
                        Feature = "素手派"
                    },
                },
                Items = new Items[]
                {
                    new Items
                    {
                        Heal  = "やくそう",
                        Doping  = "ちからのたね",
                        Weapon = "ひのきのぼう",
                        TechniqueMachines = new TechniqueMachines[]
                        {
                            new TechniqueMachines
                            {
                            Machine01 = "きあいパンチ",
                            Machine04 = "めいそう",
                            Machine08 = "ビルドアップ",
                            Machine13 = "れいとうビーム"
                            }
                        }
                    }
                }
            };

            // シリアライズ
            using (TextWriter writer = File.CreateText(savePath))
            {
                var serializer = new Serializer();
                serializer.Serialize(writer, YamlObj);
            }
        }

        /// <summary>
        /// YamlStreamを使ってYamlファイルをシリアライズする関数①
        /// </summary>
        /// <param name="yamlPath">String型：Ymalファイル出力パス</param>
        private static void writeYamlUsingStreamV1(string savePath)
        {
            var techniqueMachines = new YamlMappingNode(
                new YamlScalarNode("Machine01"), new YamlScalarNode("きあいパンチ"),
                new YamlScalarNode("Machine04"), new YamlScalarNode("めいそう"),
                new YamlScalarNode("Machine08"), new YamlScalarNode("ビルドアップ"),
                new YamlScalarNode("Machine13"), new YamlScalarNode("れいとうビーム")
            );
            var stream = new YamlStream(
                new YamlDocument(
                    new YamlMappingNode(
                        new YamlScalarNode("Version"), new YamlScalarNode("1.0.0"),
                        new YamlScalarNode("Date"), new YamlScalarNode($"{ new DateTime(1988, 2, 10) }"),
                        new YamlScalarNode("PersonInfo"),
                        new YamlMappingNode(
                            new YamlScalarNode("HP"), new YamlScalarNode("20"),
                            new YamlScalarNode("MP"), new YamlScalarNode("10"),
                            new YamlScalarNode("Attack"), new YamlScalarNode("4"),
                            new YamlScalarNode("Defence"), new YamlScalarNode("5"),
                            new YamlScalarNode("Speed"), new YamlScalarNode("2"),
                            new YamlScalarNode("Luck"), new YamlScalarNode("3")
                        ),
                        new YamlScalarNode("Classes"),
                        new YamlSequenceNode(
                            new YamlMappingNode(
                                new YamlScalarNode("Name"), new YamlScalarNode("せんし"),
                                new YamlScalarNode("Strategy"), new YamlScalarNode("ガンガンいこうぜ"),
                                new YamlScalarNode("Feature"), new YamlScalarNode("力持ち")
                            ),
                            new YamlMappingNode(
                                new YamlScalarNode("Name"), new YamlScalarNode("そうりょ"),
                                new YamlScalarNode("Strategy"), new YamlScalarNode("いのちだいじに"),
                                new YamlScalarNode("Feature"), new YamlScalarNode("癒し系")
                            ), 
                                new YamlMappingNode(
                                new YamlScalarNode("Name"), new YamlScalarNode("まほうつかい"),
                                new YamlScalarNode("Strategy"), new YamlScalarNode("じゅもんつかうな"),
                                new YamlScalarNode("Feature"), new YamlScalarNode("素手派")
                            )
                        ),
                        new YamlScalarNode("Items"),
                        new YamlMappingNode(
                            new YamlScalarNode("Heal"), new YamlScalarNode("やくそう"),
                            new YamlScalarNode("Doping"), new YamlScalarNode("ちからのたね"),
                            new YamlScalarNode("Weapon"), new YamlScalarNode("ひのきのぼう"),
                            new YamlScalarNode("TechniqueMachines"),
                            techniqueMachines
                        )
                    )
                )
            );

            // シリアライズ
            using TextWriter writer = File.CreateText(savePath);
            stream.Save(writer, false);
        }

        /// <summary>
        /// YamlStreamを使ってYamlファイルをシリアライズする関数②
        /// </summary>
        /// <param name="yamlPath">String型：Ymalファイル出力パス</param>
        private static void writeYamlUsingStreamV2(string savePath)
        {
            // 1行目を直接記述
            var version = "---\nVersion: 1.0.0\n";
            var sr = new StringReader(version);
            var stream = new YamlStream();
            stream.Load(sr);

            // 先ほど書いた1行目を根ノードとして取得
            var root_node = (YamlMappingNode)stream.Documents[0].RootNode;
            root_node.Add("Date", $"{ new DateTime(1988, 2, 10) }");

            // PersonInfo
            var personInfo = new YamlMappingNode();
            personInfo.Add("HP", "20");
            personInfo.Add("MP", "10");
            personInfo.Add("Attack", "4");
            personInfo.Add("Defence", "5");
            personInfo.Add("Speed", "2");
            personInfo.Add("Luck", "3");
            root_node.Add("PersonInfo", personInfo);

            // Classes
            var classes = new YamlSequenceNode();
            var warrior = new YamlMappingNode();
            warrior.Add("Name", "せんし");
            warrior.Add("Strategy", "ガンガンいこうぜ");
            warrior.Add("Feature", "力持ち");
            var priest = new YamlMappingNode();
            priest.Add("Name", "そうりょ");
            priest.Add("Strategy", "いのちだいじに");
            priest.Add("Feature", "癒し系");
            var mage = new YamlMappingNode();
            mage.Add("Name", "まほうつかい");
            mage.Add("Strategy", "じゅもんつかうな");
            mage.Add("Feature", "素手派");
            classes.Add(warrior);
            classes.Add(priest);
            classes.Add(mage);
            root_node.Add("Classes", classes);

            // Items
            var items = new YamlSequenceNode();
            var item = new YamlMappingNode();
            item.Add("Heal", "やくそう");
            item.Add("Doping", "ちからのたね");
            item.Add("Weapon", "ひのきのぼう");
            var techniqueMachines = new YamlSequenceNode();
            var techniqueMachine = new YamlMappingNode();
            techniqueMachine.Add("Machine01", "きあいパンチ");
            techniqueMachine.Add("Machine04", "めいそう");
            techniqueMachine.Add("Machine08", "ビルドアップ");
            techniqueMachine.Add("Machine13", "れいとうビーム");
            techniqueMachines.Add(techniqueMachine);
            item.Add("TechniqueMachines", techniqueMachines);
            items.Add(item);
            root_node.Add("Items", items);

            // シリアライズ
            using TextWriter writer = File.CreateText(savePath);
            stream.Save(writer, false);
        }

        /// <summary>
        /// オブジェクトを使ってYamlファイルをデシリアライズする関数
        /// </summary>
        /// <param name="yamlPath">String型：Ymalファイル入力パス</param>
        private static void readYamlUsingObject(string yamlPath)
        {
            // デシリアライズ
            YamlObj yamlObj = YamlImporter.Deserialize(yamlPath);

            Console.WriteLine(yamlObj.Version);
            Console.WriteLine(yamlObj.Date);
            Console.WriteLine($"{yamlObj.PersonInfo.HP}\t{yamlObj.PersonInfo.MP}\t{yamlObj.PersonInfo.Attack}\t" +
                $"{yamlObj.PersonInfo.Defence}\t{yamlObj.PersonInfo.Speed}\t{yamlObj.PersonInfo.Luck}");
            foreach (var tClass in yamlObj.Classes)
            {
                Console.WriteLine($"{tClass.Name}\t{tClass.Strategy}\t{tClass.Feature}");
            }
            foreach (var item in yamlObj.Items)
            {
                Console.WriteLine($"{item.Heal}\t{item.Doping}\t{item.Weapon}");
                foreach (var TechniqueMachine in item.TechniqueMachines)
                {
                    Console.WriteLine($"{TechniqueMachine.Machine01}\t{TechniqueMachine.Machine04}\t" +
                        $"{TechniqueMachine.Machine08}\t{TechniqueMachine.Machine13}");
                }
            }
        }

        /// <summary>
        /// YamlStreamを使ってYamlファイルをデシリアライズする関数
        /// </summary>
        /// <param name="yamlPath">String型：Ymalファイル入力パス</param>
        private static void readYamlUsingStream(string yamlPath)
        {
            // YamlStreamで読み込み
            var input = new StreamReader(yamlPath, Encoding.UTF8);
            var yaml = new YamlStream();
            yaml.Load(input);

            // ルートノードたち
            var rootNode = yaml.Documents[0].RootNode;

            var version = rootNode["Version"];
            Console.WriteLine(version);
            var date = rootNode["Date"];
            Console.WriteLine(date);

            var personInfo = (YamlMappingNode)rootNode["PersonInfo"];
            foreach (var c in personInfo.Children)
            {
                Console.WriteLine($"{c.Key} : {c.Value}");
            }

            var classes = (YamlSequenceNode)rootNode["Classes"];
            foreach (var c in classes.Children)
            {
                Console.WriteLine($"{c["Name"]}, {c["Strategy"]}, {c["Feature"]}");
            }

            /*var classes = (YamlSequenceNode)rootNode["Classes"];
            foreach (YamlMappingNode c in classes.Children)
            {
                foreach (var cc in c.Children)
                {
                    Console.WriteLine($"{cc.Key} : {cc.Value}");
                }
            }*/

            var items = (YamlSequenceNode)rootNode["Items"];
            foreach (var c in items.Children)
            {
                Console.WriteLine($"{c["Heal"]}, {c["Doping"]}, {c["Weapon"]}");
                foreach (var cc in (YamlSequenceNode)c["TechniqueMachines"])
                {
                    Console.WriteLine($"{cc["Machine01"]}, {cc["Machine04"]}, {cc["Machine08"]}, {cc["Machine13"]}");
                }
            }
        }

        public class YamlImporter
        {
            public static YamlObj Deserialize(string yamlPath)
            {
                // テキスト抽出
                using var input = new StreamReader(yamlPath, Encoding.UTF8);

                // デシリアライザインスタンス作成
                var deserializer = new Deserializer();

                // yamlデータのオブジェクトを作成
                var deserializeObject = deserializer.Deserialize<YamlObj>(input);
                
                return deserializeObject;
            }
        }

        #region Yaml object
        /*
         * 変数名をyamlファイル内のキーと一致させる必要あり
         * クラス名は任意
         */
        public class YamlObj
        {
            public string Version { get; set; }
            public DateTime Date { get; set; }
            public PersonInfo PersonInfo { get; set; }
            public Classes[] Classes { get; set; }
            public Items[] Items { get; set; }
        }

        public class PersonInfo
        {
            public int HP { get; set; }
            public int MP { get; set; }
            public int Attack { get; set; }
            public int Defence { get; set; }
            public int Speed { get; set; }
            public int Luck { get; set; }
        }

        public class Classes
        {
            public string Name { get; set; }
            public string Strategy { get; set; }
            public string Feature { get; set; }
        }

        public class Items
        {
            public string Heal { get; set; }
            public string Doping { get; set; }
            public string Weapon { get; set; }
            public TechniqueMachines[] TechniqueMachines { get; set; }
        }

        public class TechniqueMachines
        {
            public string Machine01 { get; set; }
            public string Machine04 { get; set; }
            public string Machine08 { get; set; }
            public string Machine13 { get; set; }
        }
        #endregion
    }
}
