using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EBelge.Models
{
    public class Taxpayer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string identifier;
        private string alias;
        private string title;
        private string type;
        private string register_time;
        private string unit;
        private string alias_creation_time;




        public string Identifier
        {
            get { return this.identifier; }
            set
            {
                if (value != this.identifier)
                {
                    this.identifier = value;
                    this.OnPropertyChanged("Identifier");
                }
            }
        }

        public string Alias
        {
            get { return this.alias; }
            set
            {
                if (value != this.alias)
                {
                    this.alias = value;
                    this.OnPropertyChanged("Alias");
                }
            }
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                if (value != this.title)
                {
                    this.title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        public string Type
        {
            get { return this.type; }
            set
            {
                if (value != this.type)
                {
                    this.type = value;
                    this.OnPropertyChanged("Type");
                }
            }
        }

        public string Register_time
        {
            get { return this.register_time; }
            set
            {
                if (value != this.register_time)
                {
                    this.register_time = value;
                    this.OnPropertyChanged("Register_time");
                }
            }
        }

        public string Unit
        {
            get { return this.unit; }
            set
            {
                if (value != this.unit)
                {
                    this.unit = value;
                    this.OnPropertyChanged("Unit");
                }
            }
        }

        public string Alias_creation_time
        {
            get { return this.alias_creation_time; }
            set
            {
                if (value != this.alias_creation_time)
                {
                    this.alias_creation_time = value;
                    this.OnPropertyChanged("Alias_creation_time");
                }
            }
        }

        public Taxpayer( string identifier, string alias, string title, string type, string register_time, string unit, string alias_creation_time)
        {
            this.identifier = identifier;
            this.alias = alias;
            this.title = title;
            this.type = type;
            this.register_time = register_time;
            this.unit = unit;
            this.alias_creation_time = alias_creation_time;
    }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
