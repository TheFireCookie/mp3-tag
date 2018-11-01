using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Mp3Tag.ViewModel
{
  public class ViewModelLocator
  {
    public ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
      SimpleIoc.Default.Register<MainViewModel>();
    }

    public MainViewModel Main
    {
      get
      {
        return ServiceLocator.Current.GetInstance<MainViewModel>();
      }
    }

    public static void Cleanup()
    {
      // TODO Clear the ViewModels
    }
  }
}