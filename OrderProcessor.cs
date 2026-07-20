using System;
using System.Collections.Generic;

namespace BuggyApp.Processors
{
    public class OrderProcessor
    {
        // SonarQube Smell: El campo privado debería ser 'readonly' ya que solo se inicializa aquí
        private List<string> _processedOrders = new List<string>();

        // SonarQube Smell: Complejidad cognitiva muy alta debido al anidamiento excesivo (Nested Ifs)
        public double CalculateDiscount(string customerType, double totalAmount)
        {
            if (customerType != null)
            {
                if (customerType.ToLower() == "vip")
                {
                    if (totalAmount > 500)
                    {
                        if (totalAmount > 1000)
                        {
                            return totalAmount * 0.20;
                        }
                        return totalAmount * 0.10;
                    }
                }
            }

            // SonarQube Bug: Posible NullReferenceException. 
            // Si 'customerType' es null, el primer 'if' se salta, llegando aquí y rompiendo el código al usar .ToUpper()
            string upperType = customerType.ToUpper(); 
            
            return 0;
        }

        public int ProcessBatch(int[] orderIds)
        {
            int count = 0;

            // SonarQube Bug: El bucle no avanza correctamente o terminará de inmediato (Loops should terminate)
            for (int i = 0; i < orderIds.Length; i++)
            {
                count += orderIds[i];
                
                // Un error común de lógica: meter un break accidental dentro del flujo principal
                break; 
            }

            // SonarQube Smell: Código inalcanzable (Unreachable code) debido al break o retornos previos
            int redundantCheck = count * 2; 
            return count;
        }
    }
}
