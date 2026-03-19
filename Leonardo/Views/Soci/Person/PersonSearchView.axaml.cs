using ReactiveUI.Avalonia;
using ViewModels;

namespace Leonardo;

public partial class PersonSearchView : ReactiveUserControl<PersonSearchViewModel>
{
    public PersonSearchView()
    {
        InitializeComponent();
    }
}