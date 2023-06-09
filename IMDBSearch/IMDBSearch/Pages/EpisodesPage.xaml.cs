﻿using IMDBSearch.Data;
using IMDBSearch.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMDBSearch.Pages
{
    public partial class EpisodesPage : Page
    {
        ImdbProjectContext _context = new ImdbProjectContext();
        public EpisodesPage()
        {
            InitializeComponent();
            //preload all Names and Titles
            _context.Episodes.Load();
            _context.Titles.Load();
            
            //On page initalization display top 500 movies/shows
            Search();
        }

        private void Search()
        {
            string searchTerm = textsearch.Text;
            //query, return top 500 Tvshows/Movies with the title containing your search, and all list all episodes for each Tvshow/Movie
            var query =
                from episodes in _context.Episodes
                where episodes.ParentTitle.PrimaryTitle.Contains(searchTerm)
                orderby episodes.ParentTitle.PrimaryTitle
                group episodes by episodes.ParentTitle.PrimaryTitle into newGroup
                select new {
                    Index = newGroup.Key,
                    SeCount = " Episodes: " + newGroup.Count().ToString(),
                    SeList = newGroup.ToList()
        };

            //display each TvShow/Movie in List View
            listEpisodesSearchResults.Items.Clear();
            foreach (var title in query.Take(500))
            {
                listEpisodesSearchResults.Items.Add(title);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
