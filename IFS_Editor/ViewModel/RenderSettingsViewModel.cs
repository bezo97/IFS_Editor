﻿using GalaSoft.MvvmLight;
using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IFS_Editor.ViewModel
{
    public class RenderSettingsViewModel : ObservableObject
    {
        private /*readonly*/ RenderSettings _rs;

        public RenderSettingsViewModel()
        {

        }

        public RenderSettingsViewModel(RenderSettings rs)
        {
            _rs = rs;
        }

        public RenderSettings _RS
        {
            get { return _rs; }
        }

        public int SizeX
        {
            get { return _rs.SizeX; }
            set
            {
                _rs.SizeX = value;
                RaisePropertyChanged("SizeX");
            }
        }

        public int SizeY
        {
            get { return _rs.SizeY; }
            set
            {
                _rs.SizeY = value;
                RaisePropertyChanged("SizeY");
            }
        }

        public int Oversample
        {
            get { return _rs.Oversample; }
            set
            {
                _rs.Oversample = value;
                RaisePropertyChanged("Oversample");
            }
        }

        public double Filter
        {
            get { return _rs.Filter; }
            set
            {
                _rs.Filter = value;
                RaisePropertyChanged("Filter");
            }
        }

        public int Quality
        {
            get { return _rs.Quality; }
            set
            {
                _rs.Quality = value;
                RaisePropertyChanged("Quality");
            }
        }

    }
}
