using System;
using System.Collections.Generic;

namespace nothinbutdotnetprep.collections
{
    public class MovieLibrary
    {
        IList<Movie> movies;

        public MovieLibrary(IList<Movie> list_of_movies)
        {
            movies = list_of_movies;
        }

        public IEnumerable<Movie> all_movies()
        {
			foreach (var movie in movies)
				yield return movie;
        }

        public void add(Movie movie)
        {
			foreach(var existingMovie in movies)
				if (existingMovie.title == movie.title)
					return;
			movies.Add(movie);
        }

        public IEnumerable<Movie> sort_all_movies_by_title_descending()
		{
			return Sort((x, y) => y.title.CompareTo(x.title));
        }

        public IEnumerable<Movie> all_movies_published_by_pixar()
		{
			return FindBySpec(m => ProductionStudio.Pixar == m.production_studio);
        }

        public IEnumerable<Movie> all_movies_published_by_pixar_or_disney()
		{
			return FindBySpec(m => ProductionStudio.Pixar == m.production_studio || ProductionStudio.Disney == m.production_studio);
        }

        public IEnumerable<Movie> sort_all_movies_by_title_ascending()
        {
        	return Sort((x, y) => x.title.CompareTo(y.title));
        }

        public IEnumerable<Movie> sort_all_movies_by_movie_studio_and_year_published()
        {
			//var byStudio = Sort((x, y) =>  x.production_studio.CompareTo(y.production_studio));
			//return Sort(byStudio, (x, y) => );
        }

        public IEnumerable<Movie> all_movies_not_published_by_pixar()
		{
			return FindBySpec(m => ProductionStudio.Pixar != m.production_studio);
        }

        public IEnumerable<Movie> all_movies_published_after(int year)
		{
			return FindBySpec(m => m.date_published.Year > year);
        }

        public IEnumerable<Movie> all_movies_published_between_years(int startingYear, int endingYear)
		{
			return FindBySpec(m => m.date_published.Year >= startingYear && m.date_published.Year <= endingYear);
        }

        public IEnumerable<Movie> all_kid_movies()
		{
			return FindBySpec(m => Genre.kids == m.genre);
        }

        public IEnumerable<Movie> all_action_movies()
        {
        	return FindBySpec(m => Genre.action == m.genre);
        }

		public IEnumerable<Movie> FindBySpec(Predicate<Movie> spec)
		{
			foreach (var movie in movies)
				if (spec(movie))
					yield return movie;
		}

        public IEnumerable<Movie> sort_all_movies_by_date_published_descending()
		{
			return Sort((x, y) => y.date_published.CompareTo(x.date_published));
        }

        public IEnumerable<Movie> sort_all_movies_by_date_published_ascending()
        {
        	return Sort((x, y) => x.date_published.CompareTo(y.date_published));
        }

		private IEnumerable<Movie> Sort(Func<Movie, Movie, int> comparisonStrategy)
		{
			var comparer = BuildComparer(comparisonStrategy);
			var sorted = new List<Movie>(movies);
			sorted.Sort(comparer);
			return sorted;
		}

		private IComparer<Movie> BuildComparer(Func<Movie, Movie, int> comparisonStrategy)
		{
			return new MovieComparer(comparisonStrategy);
		}

		class MovieComparer : IComparer<Movie>
		{
			private readonly Func<Movie, Movie, int> doCompare;
			public MovieComparer(Func<Movie, Movie, int> doCompare)
			{
				this.doCompare = doCompare;
			}

			public int Compare(Movie x, Movie y)
			{
				return doCompare(x, y);
			}
		}
    }
}