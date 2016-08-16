using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace ReadApp
{
    public class NoticeModel
    {
        public List<string> Tags { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }

        public Visibility EmailVisibility
        {
            get
            {
                DateTime dateParse;
                if (DateTime.TryParse(Date, out dateParse))
                {
                    if (dateParse.Year.ToString().Equals(DateTime.Now.Year.ToString()))
                    {
                        return Visibility.Visible;
                    }
                }
                return Visibility.Collapsed;

            }
        }
    }
}