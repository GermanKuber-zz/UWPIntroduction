using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel;

namespace ReadApp
{
    public class MainPageData : INotifyPropertyChanged
    {
        private const string Welcome = "Bienvenido ReadApp";
        private string _title;

        private List<ReadModel> _readModels =
            new List<ReadModel>();

        public ObservableCollection<ReadModel> ReadModels { get; set; }

        public MainPageData()
        {
            ReadModels = new ObservableCollection<ReadModel>();
            if (DesignMode.DesignModeEnabled)
            {
                //Solo se carga en el modo diseño
                for (int i = 0; i < 150; i++)
                {
                    _readModels.Add(new ReadModel { Email = $"mail@prueba{i}.com", Picture = "", Name = $"Nombre : {i}", Last = $"Apellido : {i}" });

                }
                FilterText();
            }
            else {
                LoadData();
            }
            this.Title = Welcome;
        }
        public async void LoadData()
        {
            _readModels = await ReadResitory.GetReadsAsync();
            FilterText();
        }
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title)
                    return;
                _title = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        private ReadModel _selectedRead;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadModel SelectedRead
        {
            get { return _selectedRead; }
            set
            {
                _selectedRead = value;

                if (value == null)
                    Title = Welcome;
                else
                    Title = $"{Welcome} : {_selectedRead.Name}";
                PropertyChanged?.Invoke(this,
                   new PropertyChangedEventArgs(nameof(SelectedRead)));
            }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter)
                    return;

                _filter = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Filter)));

                FilterText();
            }
        }

        private void FilterText()
        {
            if (_filter == null)
                _filter = "";

            //Se filtran los readModels por Name
            var result =
                _readModels.Where(d => d.Name.ToLowerInvariant()
                .Contains(Filter.ToLowerInvariant()))
                .ToList();
            //Se excluyen los que no aplican al filtro
            var toRemove = ReadModels.Except(result).ToList();

            //Se eliminan los que no se quieren  mostrar
            foreach (var x in toRemove)
                ReadModels.Remove(x);

            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                //Agrego a la lista
                var resultItem = result[i];
                if (i + 1 > ReadModels.Count || !ReadModels[i].Equals(resultItem))
                    ReadModels.Insert(i, resultItem);
            }
        }


    }


}
