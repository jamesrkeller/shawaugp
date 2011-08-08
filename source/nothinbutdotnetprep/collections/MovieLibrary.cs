using System;
using System.Collections.Generic;

namespace nothinbutdotnetprep.collections
{
	public class MovieLibrary
	{
		private readonly IList<Movie> movies;

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
			foreach (var existingMovie in movies)
				if (existingMovie.title == movie.title)
					return;
			movies.Add(movie);
		}

		public IEnumerable<Movie> sort_all_movies_by_title_descending()
		{
			return SortMovies((x, y) => y.title.CompareTo(x.title));
		}

		public IEnumerable<Movie> all_movies_published_by_pixar()
		{
			return Find(m => ProductionStudio.Pixar == m.production_studio);
		}

		public IEnumerable<Movie> all_movies_published_by_pixar_or_disney()
		{
			return Find(m => ProductionStudio.Pixar == m.production_studio || ProductionStudio.Disney == m.production_studio);
		}

		public IEnumerable<Movie> sort_all_movies_by_title_ascending()
		{
			return SortMovies((x, y) => x.title.CompareTo(y.title));
		}

		public IEnumerable<Movie> sort_all_movies_by_movie_studio_and_year_published()
		{
			var studiosOrderedByRating = new[]
			                                {
			                                    ProductionStudio.MGM,
			                                    ProductionStudio.Pixar,
			                                    ProductionStudio.Dreamworks,
			                                    ProductionStudio.Universal,
			                                    ProductionStudio.Disney,
			                                };
			
			var moviesByStudio = new Dictionary<ProductionStudio, List<Movie>>();
			foreach (var movie in movies)
			{
				if (!moviesByStudio.ContainsKey(movie.production_studio))
					moviesByStudio.Add(movie.production_studio, new List<Movie>());
				moviesByStudio[movie.production_studio].Add(movie);
			}

			foreach (var studio in studiosOrderedByRating)
				foreach (var movie in Sort(moviesByStudio[studio], (x, y) => x.date_published.CompareTo(y.date_published)))
					yield return movie;
		}

		public IEnumerable<Movie> all_movies_not_published_by_pixar()
		{
			return Find(m => ProductionStudio.Pixar != m.production_studio);
		}

		public IEnumerable<Movie> all_movies_published_after(int year)
		{
			return Find(m => m.date_published.Year > year);
		}

		public IEnumerable<Movie> all_movies_published_between_years(int startingYear, int endingYear)
		{
			return Find(m => m.date_published.Year >= startingYear && m.date_published.Year <= endingYear);
		}

		public IEnumerable<Movie> all_kid_movies()
		{
			return Find(m => Genre.kids == m.genre);
		}

		public IEnumerable<Movie> all_action_movies()
		{
			return Find(m => Genre.action == m.genre);
		}

		public IEnumerable<Movie> Find(Predicate<Movie> spec)
		{
			foreach (Movie movie in movies)
				if (spec(movie))
					yield return movie;
		}

		public IEnumerable<Movie> sort_all_movies_by_date_published_descending()
		{
			return SortMovies((x, y) => y.date_published.CompareTo(x.date_published));
		}

		public IEnumerable<Movie> sort_all_movies_by_date_published_ascending()
		{
			return SortMovies((x, y) => x.date_published.CompareTo(y.date_published));
		}

		private IEnumerable<Movie> SortMovies(Func<Movie, Movie, int> comparisonStrategy)
		{
			return Sort(movies, comparisonStrategy);
		}

		private IEnumerable<T> Sort<T>(IEnumerable<T> items, Func<T, T, int> comparisonStrategy)
		{
			var comparer = BuildComparer(comparisonStrategy);
			var sorted = new List<T>(items);
			sorted.Sort(comparer);
			return sorted;
		}

		private IComparer<T> BuildComparer<T>(Func<T, T, int> comparisonStrategy)
		{
			return new GenericComparer<T>(comparisonStrategy);
		}

		private class GenericComparer<T> : IComparer<T>
		{
			private readonly Func<T, T, int> doCompare;

			public GenericComparer(Func<T, T, int> doCompare)
			{
				this.doCompare = doCompare;
			}

			public int Compare(T x, T y)
			{
				return doCompare(x, y);
			}
		}
	}
}