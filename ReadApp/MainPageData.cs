using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace ReadApp
{
    public class MainPageData : INotifyPropertyChanged
    {


        #region Public Properties

        public ObservableCollection<ReadModel> ReadModels { get; set; }
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

        public ObservableCollection<ContactWrapper> Contacts = new ObservableCollection<ContactWrapper>();

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
                    Title = $"Usuario Seleccionado : {_selectedRead.Name}";
                PropertyChanged?.Invoke(this,
                   new PropertyChangedEventArgs(nameof(SelectedRead)));
                UpdateContacts();
            }
        }
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
        #endregion


        #region Commands



        #endregion


        #region Private Properties

        private const string Welcome = "Bienvenido ReadApp";
        private string _title;

        private List<ReadModel> _readModels = new List<ReadModel>();

        private ReadModel _selectedRead;

        private string _filter;
        
       
        #endregion


        #region Constructor

        public MainPageData()
        {
            ReadModels = new ObservableCollection<ReadModel>();
            if (DesignMode.DesignModeEnabled)
            {

                GenerateDummyData();
            }
            else
            {
                LoadData();
            }
            this.Title = Welcome;
        }

        private void GenerateDummyData()
        {
//Solo se carga en el modo diseño
            for (int i = 0; i < 150; i++)
            {
                var readModel = new ReadModel
                {
                    Email = $"mail@prueba{i}.com",
                    Picture = "http://placehold.it/400x400",
                    Name = $"Nombre : {i}",
                    Last = $"Apellido : {i}",
                    Notices = new List<NoticeModel>
                    {
                        new NoticeModel
                        {
                            Date = RandomDay().ToString(),
                            Id = i,
                            Tags = new List<string>
                            {
                                $"Tag - {i}",
                                $"Tag - {i - 5}"
                            },
                            Text =
                                "Ex cupidatat culpa consequat enim laborum in deserunt anim occaecat. Deserunt eiusmod quis occaecat id deserunt est voluptate do fugiat adipisicing. Ut laboris in magna adipisicing amet non nulla in. Duis irure qui mollit ea et amet esse tempor dolor reprehenderit do.",
                            Title = $"Titulo numero  : {i}",
                            Image = "http://placehold.it/400x400"
                        },
                        new NoticeModel
                        {
                            Date = RandomDay().ToString(),
                            Id = i,
                            Tags = new List<string>
                            {
                                $"Tag - {i}",
                                $"Tag - {i - 5}"
                            },
                            Text =
                                "Ex cupidatat culpa consequat enim laborum in deserunt anim occaecat. Deserunt eiusmod quis occaecat id deserunt est voluptate do fugiat adipisicing. Ut laboris in magna adipisicing amet non nulla in. Duis irure qui mollit ea et amet esse tempor dolor reprehenderit do.",
                            Title = $"Titulo numero  : {i}", 
                            Image = "http://placehold.it/400x400"
                        },
                        new NoticeModel
                        {
                            Date = RandomDay().ToString(),
                            Id = i,
                            Tags = new List<string>
                            {
                                $"Tag - {i}",
                                $"Tag - {i - 5}"
                            },
                            Text =
                                "Ex cupidatat culpa consequat enim laborum in deserunt anim occaecat. Deserunt eiusmod quis occaecat id deserunt est voluptate do fugiat adipisicing. Ut laboris in magna adipisicing amet non nulla in. Duis irure qui mollit ea et amet esse tempor dolor reprehenderit do.",
                            Title = $"Titulo numero  : {i}",
                            Image = "http://placehold.it/400x400"
                        },
                        new NoticeModel
                        {
                            Date = RandomDay().ToString(),
                            Id = i,
                            Tags = new List<string>
                            {
                                $"Tag - {i}",
                                $"Tag - {i - 5}"
                            },
                            Text =
                                "Ex cupidatat culpa consequat enim laborum in deserunt anim occaecat. Deserunt eiusmod quis occaecat id deserunt est voluptate do fugiat adipisicing. Ut laboris in magna adipisicing amet non nulla in. Duis irure qui mollit ea et amet esse tempor dolor reprehenderit do.",
                            Title = $"Titulo numero  : {i}",
                            Image = "http://placehold.it/400x400"
                        }
                    }
                };

              
                ReadModels.Add(readModel);
            }
            if (ReadModels != null && ReadModels.Count > 0)
                this.SelectedRead = ReadModels.First();
            //FilterText();
        }

        #endregion


        #region Public Methods

        public async Task SendEmailAsync(NoticeModel notice)
        {
            EmailAdmin emailAdmin = new EmailAdmin();
            ContactAdmin contactAdmin = new ContactAdmin();
            var contact = await contactAdmin.GetAllContacts();
            if (notice.Title != null)
                await emailAdmin.SendEmailAsync(contact.First(x => x.Contact.Emails?.Count > 0).Contact, notice.Title,notice.Text);
        }

        #endregion


        #region Private Methods

        private void GenerateDataDummy()
        {

        }

        private DateTime RandomDay()
        {

            Random gen = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
        private async void LoadData()
        {
            _readModels = await ReadResitory.GetReadsAsync();
            FilterText();

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

        private async void UpdateContacts()
        {
            
        }

        #endregion


        #region Events Overrides



        #endregion



    }
}
