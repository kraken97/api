using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Task1.Models;

public class MovieGenreViewModel
    {
        public List<Movie> movies;
        public SelectList genres;
        public string movieGenre { get; set; }
    }