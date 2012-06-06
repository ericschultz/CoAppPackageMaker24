﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CoApp.Developer.Toolkit.Scripting.Languages.PropertySheet;
using CoApp.Packaging;
using CoApp.Packaging.Client;
using CoAppPackageMaker.ViewModels.RuleViewModels;


namespace CoAppPackageMaker.ViewModels.Base
{
    class MainWindowViewModel:ViewModelBase
    {
        private string _ruleNameSelectedItem;
        private ObservableCollection<string> _ruleNames;
        private ObservableCollection<string> _roleNames;
        //the key is the name of the rule property, the list contains values
        private Dictionary<string, List<string>> _dictionaryHistory;
        private PackageViewModel _packageViewModel;
        private MetadataViewModel _metadataViewModel;
        private RequiresViewModel _requiresViewModel;
        private PackageCompositionViewModel _packageCompositionViewModel;
        private FilesViewModel _filesViewModel;
        private ManifestViewModel _manifestViewModel;
       
        public MainWindowViewModel()
        {
          //  PathToFile = "D:\\P\\glib\\COPKG\\glib.autopkg";
          //  PackageReader reader = new PackageReader();
          //  reader.Read(PathToFile);

          //  _ruleNames = reader.Rules;
          //  Dictionary<string, IFactory> dic = new Dictionary<string, IFactory>();
          //  dic.Add("package", new PackageViewModelFactory());
          //  dic.Add("files",new FilesViewModelFactory());
          //  foreach (string str in _ruleNames)
          //  {
          //      if (dic.ContainsKey(str))
          //      {
          //          object instance = (dic[str].CreateInstance(reader));
          //          string name = instance.GetType().Name;
          //          var p = GetType().GetProperty(name);
          //          p.SetValue(this, instance, null);
          //      }

          //}

         // PathToFile = "D:\\P\\COPKG\\test2.autopkg";
     // PathToFile = "D:\\P\\procmon\\copkg\\procmon.autopkg";
        PathToFile = "D:\\P\\glib\\COPKG\\glib.autopkg";

          
         if (PathToFile != null && File.Exists(PathToFile))
         {
           
         LoadData();
         }  
           
        }

        private void LoadData()
        {
            _dictionaryHistory = new Dictionary<string, List<string>>();
            PackageReader reader = new PackageReader();
            reader.Read(PathToFile);
            ManifestViewModel k = new ManifestViewModel(reader);
            _manifestViewModel = k;
            SigningViewModel signingViewModel= new SigningViewModel(reader);
            _signingViewModel = signingViewModel;
            RequiresViewModel requiresViewModel=new RequiresViewModel(reader);
            _requiresViewModel = requiresViewModel;
            _defineViewModel=new DefineViewModel(reader);
            _licenseViewModel=new LicenseViewModel(reader);
            _packageCompositionViewModel = new PackageCompositionViewModel()
            
            {
                Symlinks = reader.GetRulesPropertyValueByName("package-composition", "symlinks")
            };
            _ruleNames = reader.Rules;
            foreach (string str in _ruleNames)
            {
                CreateModels(reader, str);
            }

        }

        private  string _pathToFile;
        public string PathToFile
        {
            get { return _pathToFile; }
            set
            {
                _pathToFile = value;
                OnPropertyChanged("PathToFile");
            }
        }

        
        private void CreateModels(PackageReader reader, string name)
        {
            switch (name)
            {
                case "package":
                    _packageViewModel = new PackageViewModel(reader);
                    break;
                case "metadata":
                    _metadataViewModel = new MetadataViewModel(reader);
                  break;
                case "files":
                  _filesViewModel = new FilesViewModel(reader);
                 break;
                default:
                    Console.WriteLine("Invalid name");
                    break;
            }



            _requiresViewModel = new RequiresViewModel(reader);
         

            _packageCompositionViewModel = new PackageCompositionViewModel()
            {
                Symlinks = reader.GetRulesPropertyValueByName("package-composition", "symlinks")
            };


            _packageCompositionViewModel = new PackageCompositionViewModel()
            {
                Symlinks = reader.GetRulesPropertyValueByName("faux-pax", "downloads")
            };
           
        }

        #region ViewModels

      
        public PackageViewModel PackageViewModel
        {
            get
            {
                return _packageViewModel;
            }
            set
            {
                _packageViewModel = value;
                OnPropertyChanged("PackageViewModel");
            }
        }


        public MetadataViewModel MetadataViewModel
        {
            get
            {
                return _metadataViewModel;
            }
            set
            {
                _metadataViewModel = value;
                OnPropertyChanged(" MetadataViewModel");
            }
        }

        public RequiresViewModel RequiresViewModel
        {
            get
            {
                return _requiresViewModel;
            }
            set
            {
                _requiresViewModel = value;
                OnPropertyChanged("RequiresViewModel");
            }
        }

        public PackageCompositionViewModel PackageCompositionViewModel
        {
            get
            {
                return _packageCompositionViewModel;
            }
            set
            {
               _packageCompositionViewModel = value;
                OnPropertyChanged("PackageCompositionViewModel");
            }
        }

        public FilesViewModel FilesViewModel
        {
            get
            {
                return _filesViewModel;
            }
            set
            {
                _filesViewModel = value;
                OnPropertyChanged("FilesViewModel");
            }
        }

        public ManifestViewModel ManifestViewModel
        {
            get
            {
                return _manifestViewModel;
            }
            set
            {
               _manifestViewModel = value;
                OnPropertyChanged("ManifestViewModel");
            }
        }

        private SigningViewModel _signingViewModel;
        public SigningViewModel SigningViewModel
        {
            get
            {
                return _signingViewModel;
            }
            set
            {
                _signingViewModel = value;
                OnPropertyChanged("SigningViewModel");
            }
        }

        private DefineViewModel _defineViewModel;

        public DefineViewModel DefineViewModel
        {
            get { return _defineViewModel; }
            set
            {
                _defineViewModel = value;
                OnPropertyChanged("DefineViewModel");
            }
        }

        private LicenseViewModel _licenseViewModel;

        public LicenseViewModel LicenseViewModel
        {
            get { return _licenseViewModel; }
            set
            {
                _licenseViewModel = value;
                OnPropertyChanged("LicenseViewModel");
            }
        }

        

        #endregion ViewModels

       

       
         public ObservableCollection<string> RulesNames
         {
             get { return _ruleNames; }
             set
             {
                 _ruleNames = value;
                 OnPropertyChanged("RulesNames");
             }
         }

         public ObservableCollection<string> RolesNames
         {
             get { return _roleNames; }
             set
             {
                 _roleNames = value;
                 OnPropertyChanged("RolesNames");
             }
         }

        //used for binding between Rule and it's properties and values
         public String RuleNameSelectedItem
         {
             get { return _ruleNameSelectedItem; }
             set
             {
                 _ruleNameSelectedItem = value;
                 OnPropertyChanged("RuleNameSelectedItem");
             }
         }

        

       

        
        
         #region Commands

         public ICommand DeleteCommand
         {
             get { return new RelayCommand(DeleteExecute, CanDeleteExecute); }
         }

        public ICommand SaveCommand
         {
             get { throw new NotImplementedException(); }
         }

         public ICommand ResetCommand
         {
             get { throw new NotImplementedException(); }
         }

         public ICommand NewCommand
         {
             get { return new RelayCommand(NewExecute, CanNewExecute); }
         }

         public ICommand UndoCommand
         {
             get { throw new NotImplementedException(); }
         }

         public ICommand RedoCommand
         {
             get { throw new NotImplementedException(); }
         }


#endregion


         #region methods

         void DeleteExecute()
         {
             if (CanDeleteExecute() )
             {
                 try
                 {
                     _ruleNames.Remove(this._ruleNameSelectedItem);

                     // ResetForm();
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
             }
         }


        bool CanDeleteExecute()
         {
          return this.RuleNameSelectedItem!=null;
            
         }


         void NewExecute()
         {
             if (CanNewExecute())
             {
                 try
                 {
                     _ruleNames.Add(this._ruleNameSelectedItem);

                     // ResetForm();
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
             }
         }


         bool CanNewExecute()
         {
             //to add if rule properties have correct values
             return this.RuleNameSelectedItem != null;
         }

#endregion
    }
}
