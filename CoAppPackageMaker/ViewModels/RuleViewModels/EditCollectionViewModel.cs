﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CoAppPackageMaker.ViewModels.Base;
using MonitoredUndo;

namespace CoAppPackageMaker.ViewModels.RuleViewModels
{
    public class EditCollectionViewModel:ExtraPropertiesViewModelBase
    {
        PackageReader _reader;
        private ObservableCollection<ItemViewModel> _editableItems;
        private ItemViewModel.Process _updateSource; 
        public ObservableCollection<ItemViewModel> EditableItems
        {
            get { return _editableItems; }
            set
            {
               
                _editableItems = value;
                OnPropertyChanged("EditableItems");
            }
        }

        public EditCollectionViewModel(PackageReader reader, MainWindowViewModel root, ObservableCollection<ItemViewModel> collection)
        {
            _reader = reader;
            //how to update value and source value? 
            //to pass the collection or viewModel() in order to ovveride update?
            Root = root;
            _editableItems = collection;
            _updateSource = this.EditableItems.FirstOrDefault().UpdateSource;
            _editableItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FilesCollectionCollectionChanged);
        }

        void FilesCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DefaultChangeFactory.OnCollectionChanged(this, "EditableItems", EditableItems, e);
            OnPropertyChanged("EditableItems");
        }
      
        private ItemViewModel _selectedItem;
        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        
        #region Event Handlers

        public ICommand RemoveCommand
        {
            get { return new RelayCommand(Remove, CanRemove); }
        }

        public void Remove()
        {
            this.EditableItems.Remove(this.SelectedItem);
           
        }
      
           bool CanRemove()
         {
             return this.SelectedItem != null;
            
         }
          

           public ICommand AddCommand
           {
               get { return new RelayCommand(Add, CanAdd); }
           }

           bool CanAdd()
           {
               return this.EditableItems!= null;

           }
           public void Add()
           {

               this.EditableItems.Add(new ItemViewModel() { Root = this.Root, Reader = _reader, UpdateSource = _updateSource });
           }
    
        #endregion
    }
}