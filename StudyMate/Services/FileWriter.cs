using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudyMate.Models;

namespace StudyMate.Services
{
	public class FileWriter : IFileWriter
	{
		public ICollection<QandA> QandAs(IFormFile file, int id)
		{
			string message = "";
			string correctAnswer = "";
			string topic = "";
			string quest = "";
			string opt1 = "";
			string opt2 = "";
			string opt3 = "";
			string opt4 = "";
			string explanation = "";
			int explain = 0;
			int question = 0;
			int option1 = 0;
			int option2 = 0;
			int option3 = 0;
			int option4 = 0;
			int topicMain = 0;

			List<QandA> multipleQuestions = new List<QandA>();
			Stream reader = file.OpenReadStream();
			if (reader != null)
			{
				try
				{
					using (StreamReader streamReader = new StreamReader(reader))
					{

						foreach (var item in System.IO.File.ReadLines(file.FileName))
						{
							if (string.IsNullOrWhiteSpace(item))
							{ }
							else {
							
								if (item.EndsWith('#') && question == 0)
								{

									quest += item.Remove(item.Length - 1, 1);
									question = 1;
								}



								else if (item.EndsWith(',') && option1 == 0)
								{

									opt1 += item.Remove(item.Length - 1, 1);
									if (item.StartsWith('/'))
									{
										opt1 = opt1.Remove(0, 1);
										correctAnswer = opt1;
									}
									option1 = 1;
								}


								else if (item.EndsWith(',') && option2 == 0)
								{

									opt2 += item.Remove(item.Length - 1, 1);
									if (item.StartsWith('/'))
									{
										opt2 = opt2.Remove(0, 1);
										correctAnswer = opt2;
									}
									option2 = 1;

								}

								else if (item.EndsWith(',') && option3 == 0)
								{

									opt3 += item.Remove(item.Length - 1, 1);
									if (item.StartsWith('/'))
									{
										opt3 = opt3.Remove(0, 1);
										correctAnswer = opt3;
									}
									option3 = 1;

								}
								else if (item.EndsWith(',') && option4 == 0)
								{

									opt4 += item.Remove(item.Length - 1, 1);
									if (item.StartsWith('/'))
									{
										opt4 = opt4.Remove(0, 1);
										correctAnswer = opt4;
									}
								}

								else if (item.EndsWith('@') && explain == 0)
								{
									topic += item.Remove(item.Length - 1, 1);
									
								}

								else if (item.EndsWith('$') && topicMain == 0)
								{
									explanation += item.Remove(item.Length - 1, 1);
									QandA qa = new QandA() { Question = quest, OptionA = opt1, OptionB = opt2, OptionC = opt3, OptionD = opt4, TopicTitle = topic, CorrectAnswer = correctAnswer, Explanation = explanation, CourseID = id };
									multipleQuestions.Add(qa);
									explanation = "";
									topic = "";
									quest = "";
									opt1 = "";
									opt2 = "";
									opt3 = "";
									opt4 = "";
									topicMain = 0;
									explain = 0;
									question = 0;
									option1 = 0;
									option2 = 0;
									option3 = 0;
								}
							}
							
						}
					}

				}
				catch (Exception e)
				{
					message = e.Message;

				}
				finally
				{
					reader.Close();

				}
			}
			if (string.IsNullOrEmpty(message))
			{
				return multipleQuestions;
			}
			else return null;
		}
	
	}
}
