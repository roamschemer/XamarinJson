using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using XamarinJson.Models;

namespace XamarinJson.ViewModels {
    public class MainPageViewModel : ViewModelBase {
        public ReadOnlyReactiveCollection<CategoryViewModel> CategorieViewModels { get; }
        public ReactiveCommand<object> SampleSetCommand { get; }
        public ReactiveCommand<object> LoadCommand { get; }
        public ReactiveCommand<object> SaveCommand { get; }
        public MainPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            coreModel.Load();
            CategorieViewModels = coreModel.Categorys.ToReadOnlyReactiveCollection(x => new CategoryViewModel(coreModel, x)).AddTo(this.Disposable);
            SampleSetCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.CleateSample());
            LoadCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.Load());
            SaveCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.Save());
        }

        public class CategoryViewModel : IDisposable {
            public ReactiveProperty<string> Name { get; }
            public ReadOnlyReactiveCollection<PersonViewModel> PersonViewModels { get; }
            public ReactiveCommand<object> UpCommand { get; }
            public ReactiveCommand<object> DownCommand { get; }
            public ReactiveCommand<object> DeleteCommand { get; }
            public CategoryViewModel(CoreModel coreModel, Category category) {
                Name = category.ObserveProperty(x => x.Name).ToReactiveProperty().AddTo(this.Disposable);
                PersonViewModels = category.Persons.ToReadOnlyReactiveCollection(x => new PersonViewModel(category, x)).AddTo(this.Disposable);
                UpCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.UpCategory(category));
                DownCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.DownCategory(category));
                DeleteCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.DeleteCagegory(category));
            }
            //後始末
            private CompositeDisposable Disposable { get; } = new CompositeDisposable();
            public void Dispose() => this.Disposable.Dispose();

            public class PersonViewModel {
                public ReactiveProperty<string> Name { get; }
                public ReactiveCommand<object> UpCommand { get; }
                public ReactiveCommand<object> DownCommand { get; }
                public ReactiveCommand<object> DeleteCommand { get; }
                public PersonViewModel(Category category, Person person) {
                    Name = person.ObserveProperty(x => x.Name).ToReactiveProperty().AddTo(this.Disposable);
                    UpCommand = new ReactiveCommand().WithSubscribe(_ => category.UpCategory(person));
                    DownCommand = new ReactiveCommand().WithSubscribe(_ => category.DownCategory(person));
                    DeleteCommand = new ReactiveCommand().WithSubscribe(_ => category.DeleteCagegory(person));
                }
                //後始末
                private CompositeDisposable Disposable { get; } = new CompositeDisposable();
                public void Dispose() => this.Disposable.Dispose();
            }
        }
    }

}
