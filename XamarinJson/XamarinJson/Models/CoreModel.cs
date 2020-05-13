using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace XamarinJson.Models {

    public class Person : BindableBase {

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string name;

    }

    public class Category : BindableBase {

        public string Name {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string name;

        public ObservableCollection<Person> Persons { get; set; }

        public void CleateCategory() {
            Persons.Add(new Person() { Name = "人物" });
        }

        public void UpCategory(Person person) {
            var item = Persons.Select((model, index) => (model, index)).First(x => x.model == person);
            if (item.index == 0) return;
            Persons.Move(item.index, item.index - 1);
        }

        public void DownCategory(Person person) {
            var item = Persons.Select((model, index) => (model, index)).First(x => x.model == person);
            if (item.index == Persons.Count - 1) return;
            Persons.Move(item.index, item.index + 1);
        }

        public void DeleteCagegory(Person person) {
            if (Persons.Count == 1) return;
            Persons.Remove(person);
        }
    }

    public class CoreModel : BindableBase {

        private readonly string _fileName = "XamarinJsonTest.json";

        public ObservableCollection<Category> Categorys { get; set; }

        public void Save() {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName);
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(Categorys, options);
            File.WriteAllText(fileName, json);
        }

        public void Load() {
            Categorys = new ObservableCollection<Category>();
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName);
            if (!File.Exists(fileName)) {
                CleateCategory();
                return;
            }
            var json = File.ReadAllText(fileName);
            var coreModel = JsonSerializer.Deserialize<CoreModel>(json);
            Categorys = coreModel.Categorys;
        }

        public void CleateCategory() {
            Categorys.Add(new Category() {
                Name = "カテゴリ",
                Persons = new ObservableCollection<Person>() {
                    new Person() { Name = "人物" }
                }
            });
        }

        public void UpCategory(Category category) {
            var item = Categorys.Select((model, index) => (model, index)).First(x => x.model == category);
            if (item.index == 0) return;
            Categorys.Move(item.index, item.index - 1);
        }

        public void DownCategory(Category category) {
            var item = Categorys.Select((model, index) => (model, index)).First(x => x.model == category);
            if (item.index == Categorys.Count - 1) return;
            Categorys.Move(item.index, item.index + 1);
        }

        public void DeleteCagegory(Category category) {
            if (Categorys.Count == 1) return;
            Categorys.Remove(category);
        }

        public void CleateSample() {
            Categorys = Aveter2ten0();
        }

        private static ObservableCollection<Category> Aveter2ten0() =>
            new ObservableCollection<Category>(){
                new Category() {
                    Name = "ひそうら", Persons = new ObservableCollection<Person>() {
                        new Person(){Name = "白石ひなた"},
                        new Person(){Name = "都三代らみょん"},
                        new Person(){Name = "三田そにあ"},
                        new Person(){Name = "縁うか"}
                    }
                },
                new Category() {
                    Name = "九条家", Persons = new ObservableCollection<Person>() {
                        new Person(){Name = "九条林檎"},
                        new Person(){Name = "九条棗"},
                        new Person(){Name = "九条杏子"},
                        new Person(){Name = "九条茘枝"},
                    }
                },
                new Category() {
                    Name = "ほうれん草ず", Persons = new ObservableCollection<Person>() {
                        new Person() { Name = "白乃クロミ"},
                        new Person() { Name = "碧惺スキア"},
                        new Person() { Name = "紫吹ふうか"},
                        new Person() { Name = "菜花なな"},
                    }
                },
                new Category() {
                    Name = "フカヒレシスターズ", Persons = new ObservableCollection<Person>() {
                        new Person() { Name = "巻乃もなか"},
                        new Person() { Name = "幸糖ミュウミュウ"},
                        new Person() { Name = "青咲ローズ"},
                        new Person() { Name = "泡沫調"},
                    }
                },
                new Category() {
                    Name = "ユイしあ", Persons = new ObservableCollection<Person>() {
                        new Person() { Name = "結目ユイ"},
                        new Person() { Name = "水瀬しあ"},
                    }
                },
            };
    }
}
