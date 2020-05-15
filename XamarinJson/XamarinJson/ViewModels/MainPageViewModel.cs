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
        public ReadOnlyReactiveCollection<CategoryViewModel> CategoryViewModels { get; }
        public ReactiveCommand<object> SampleSetCommand { get; }
        public ReactiveCommand<string> LoadCommand { get; }
        public ReactiveCommand<string> SaveCommand { get; }
        public ReactiveCommand<object> AddCommand { get; }
        public ReactiveCommand<object> ClearCommand { get; }
        public ReactiveProperty<CategoryViewModel> SelectedCategoryViewModel { get; }
        public MainPageViewModel(INavigationService navigationService, CoreModel coreModel) : base(navigationService) {
            coreModel.Load("XamarinJson1");
            CategoryViewModels = coreModel.Categorys.ToReadOnlyReactiveCollection(x => new CategoryViewModel(coreModel, x)).AddTo(this.Disposable);
            SelectedCategoryViewModel = new ReactiveProperty<CategoryViewModel>(CategoryViewModels.First());
            LoadCommand = new ReactiveCommand<string>().WithSubscribe(x => coreModel.Load(x));
            SaveCommand = new ReactiveCommand<string>().WithSubscribe(x => coreModel.Save(x));
            SampleSetCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.CreateSample());
            AddCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.AddCategory());
            ClearCommand = new ReactiveCommand().WithSubscribe(_ => { coreModel.Clear(); SelectedCategoryViewModel.Value = null; });
        }

        public class CategoryViewModel : IDisposable {
            public ReactiveProperty<string> Name { get; }
            public ReadOnlyReactiveCollection<PersonViewModel> PersonViewModels { get; }
            public ReactiveCommand<object> UpCommand { get; }
            public ReactiveCommand<object> DownCommand { get; }
            public ReactiveCommand<object> DeleteCommand { get; }
            public ReactiveCommand<object> AddCommand { get; }
            public ReactiveCommand<object> ClearCommand { get; }
            public CategoryViewModel(CoreModel coreModel, Category category) {
                Name = category.ToReactivePropertyAsSynchronized(x => x.Name).AddTo(this.Disposable);
                PersonViewModels = category.Persons.ToReadOnlyReactiveCollection(x => new PersonViewModel(category, x)).AddTo(this.Disposable);
                UpCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.UpCategory(category));
                DownCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.DownCategory(category));
                DeleteCommand = new ReactiveCommand().WithSubscribe(_ => coreModel.DeleteCagegory(category));
                AddCommand = new ReactiveCommand().WithSubscribe(_ => category.AddPerson());
                ClearCommand = new ReactiveCommand().WithSubscribe(_ => category.Clear());
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
                    Name = person.ToReactivePropertyAsSynchronized(x => x.Name).AddTo(this.Disposable);
                    UpCommand = new ReactiveCommand().WithSubscribe(_ => category.UpPerson(person));
                    DownCommand = new ReactiveCommand().WithSubscribe(_ => category.DownPerson(person));
                    DeleteCommand = new ReactiveCommand().WithSubscribe(_ => category.DeletePerson(person));
                }
                //後始末
                private CompositeDisposable Disposable { get; } = new CompositeDisposable();
                public void Dispose() => this.Disposable.Dispose();
            }
        }
    }

}
