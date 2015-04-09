// -------------------------------------------------------------------------------------------
// <copyright file="LuceneAnalyzer.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2015
// </copyright>
// -------------------------------------------------------------------------------------------
// Copyright 2015 Sitecore Corporation A/S
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
// except in compliance with the License. You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the 
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions 
// and limitations under the License.
// -------------------------------------------------------------------------------------------

namespace Sitecore.Ecommerce.Search
{
  using System.IO;
  using Lucene.Net.Analysis;
  using Lucene.Net.Analysis.Standard;

  /// <summary>
  /// Analyzer class
  /// </summary>
  public class LuceneAnalyzer : Analyzer
  {
    /// <summary>
    /// Saved Streams class
    /// </summary>
    private sealed class SavedStreams
    {
      /// <summary>
      ///  filtered token stream
      /// </summary>
      internal TokenStream FilteredTokenStream;

      /// <summary>
      /// Token Stream
      /// </summary>
      internal WhitespaceTokenizer TokenStream;

      /// <summary>
      /// Initializes a new instance of the <see cref="SavedStreams"/> class.
      /// </summary>
      public SavedStreams()
      {

      }
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LuceneAnalyzer"/> class.
    /// </summary>
    public LuceneAnalyzer()
    {
    }

    /// <summary>
    /// Reusables the token stream.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="reader">The reader.</param>
    /// <returns>Returns TokenStream</returns>
    [NotNull]
    public override TokenStream ReusableTokenStream([NotNull] string fieldName, [NotNull] TextReader reader)
    {
      SavedStreams previousTokenStream = (SavedStreams)this.PreviousTokenStream;
      if (previousTokenStream == null)
      {
        previousTokenStream = new SavedStreams();
        this.PreviousTokenStream = previousTokenStream;
        previousTokenStream.TokenStream = new WhitespaceTokenizer(reader);
        previousTokenStream.FilteredTokenStream = new StandardFilter(previousTokenStream.TokenStream);
        previousTokenStream.FilteredTokenStream = new LowerCaseFilter(previousTokenStream.FilteredTokenStream);
      }
      else
      {
        previousTokenStream.TokenStream.Reset(reader);
      }

      return previousTokenStream.FilteredTokenStream;
    }

    /// <summary>
    /// Converts the <see cref="LuceneAnalyzer"/> to a <see cref="TokenStream"/>.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="reader">The reader.</param>
    /// <returns>The <see cref="TokenStream"/>.</returns>
    [NotNull]
    public override TokenStream TokenStream([NotNull] string fieldName, [NotNull] TextReader reader)
    {
      Diagnostics.Assert.ArgumentNotNull(fieldName, "fieldName");
      Diagnostics.Assert.ArgumentNotNull(reader, "reader");

      return new WhitespaceTokenizer(reader);
    }
  }
}