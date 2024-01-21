using Chuma.ChumaClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChumaClasses.Chuma
{
    public class ChumaModel : IChumaModel, IPaintInfo
    {
        private const int CRITICAL_DISTANCE = 10;

        public readonly HashSet<Agent> agents = new HashSet<Agent>(); // множество агентов модели
        public void AddAgent(Agent agent) => agents.Add(agent);
        public void RemoveAgent(Agent agent) => agents.Remove(agent);
        public bool IsCriticalDistance(Agent firstAgent, Agent secondAgent)
            => Math.Sqrt(Math.Pow(firstAgent.X - secondAgent.X, 2) + Math.Pow(firstAgent.Y - secondAgent.Y, 2)) <= CRITICAL_DISTANCE;

        /// <summary>
        /// Обновляет положение агентов
        /// </summary>
        /// <param name="timePassed"></param>
        public void UpdateAgents(double timePassed)
        {
            // обновляем положение всех агентов
            foreach (var agent in agents)
            {
                agent.UpdatePosition(timePassed);

                // проверяем наличие зараженных и лечим их
                foreach (var otherAgent in agents)
                {
                    if (agent != otherAgent // это не тот же самый агент
                        && IsCriticalDistance(agent, otherAgent)) // расстояние, на котором можно заразиться
                    {
                        if (agent is Person person) // первый - это человек?
                        {
                            if (otherAgent is Doctor doctor) // второй доктор? 
                            {
                                doctor.TryHeal(person); // если да - пробуем лечить первого
                            }
                            else // оба люди
                            {
                                // пытаются заразить друг друга
                                person.TryInfect(otherAgent);
                                ((Person)otherAgent).TryInfect(person);
                            }
                        }
                        else // это был доктор
                        {
                            if(otherAgent is Person otherPerson) // второй - человек?
                                ((Doctor)agent).TryHeal(otherPerson); // если да, то пробуем лечить его
                            // иначе это два доктора (лечить и заражать никого не нужно)
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Печатает текущее состояние всех агентов
        /// </summary>
        /// <returns></returns>
        public StringBuilder PrintCurrentStateAllAgents()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var agent in agents)
            {
                if (agent is Doctor doctor)
                    sb.AppendLine($"Доктор {doctor.id}:").AppendLine($"({doctor.X}, {doctor.Y})");
                else
                {
                    var person = agent as Person;
                    var isInfected = person.IsInfected ? "Да" : "Нет";
                    sb.AppendLine($"Человек {person.id}:").AppendLine($"{person.X}, {person.Y}), Заражен: {isInfected}");
                }
            }

            sb.AppendLine("--------------");

            return sb;
        }

        /// <summary>
        /// Вернёт цвет, которым будет раскрашено соответствующее состояние агента ( 0 - заражен, 1 - здоров, 2 - доктор)
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public int GetColorAgent(Agent agent) => agent is Person person ? person.IsInfected ? 0 : 1 : 2;

        /// <summary>
        /// Получить текущую "карту состояния" для раскраски
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, (int, int, int)> GetCurrentPaintInfo()
        {
            Dictionary<string, (int, int, int)> dic = new Dictionary<string, (int, int, int)>();

            foreach (var agent in agents)
            {
                dic.Add(agent.id.ToString(), (agent.X, agent.Y, GetColorAgent(agent)));
            }

            return dic;
        }
    }
}