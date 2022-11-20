using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Dynamic;


namespace ConsoleApp1
{
    internal class Program
    {
        public class All_game
        {
            public interface IFab
            {
                public void build(Player player, int current_step);
            }
            class Fabric : IFab
            {
                int step;
                public Fabric(int current_step)
                {
                    step = current_step + 5;
                }
                public Fabric()
                { }
                public void build(Player player, int current_step)
                {
                    if (step == current_step)
                    {
                        player.all_fabs.Add(new Fabric());
                    }
                }
            }
            class AutomatedFabric : IFab
            {
                int step;
                public AutomatedFabric(int current_step)
                {
                    step = current_step + 7;
                }
                public AutomatedFabric()
                { }
                public void build(Player player, int current_step)
                {
                    if (step == current_step)
                    {
                        player.all_fabs.Add(new AutomatedFabric());
                    }
                }
              
            }
            public class Player
            {
                public int id, balance, ESM, EGP;
                public List<IFab> all_fabs = new List<IFab>();
                public List<IFab> fabs_on_building = new List<IFab>();
                public List<int[]> loans = new List<int[]>();
                public int fabrics, autfabrics;
                int tax4P = 500;
                int tax4M = 300;
                int tax4Fabric = 1000;
                int tax4AutomatedFabric = 1500;
                public Player(int id)
                {
                    Fabric fab1 = new Fabric();
                    Fabric fab2 = new Fabric();
                    all_fabs.Add(fab1);
                    all_fabs.Add(fab2);
                    this.id = id;
                    balance = 10000;
                    ESM = 4;
                    EGP = 2;
                }
                public void buildUsual(int current_step)
                {
                    if (balance > 5000)
                    {
                        balance -= 5000;
                        fabs_on_building.Add(new Fabric(current_step));
                        Console.WriteLine($"Вы успешно поставили фабрику на строительство!\nВаш остаток на балансе: {balance}.");// pomenyat
                    }
                    else { Console.WriteLine("Недостаточно денег на балансе!"); }
                }
                public void buildAutomated(int current_step)
                {
                    if (balance > 10000)
                    {
                        balance -= 10000;
                        fabs_on_building.Add(new AutomatedFabric(current_step));
                        Console.WriteLine($"Вы успешно поставили автоматизированную фабрику на строительство!\nВаш остаток на балансе: {balance}.");
                    }
                    else { Console.WriteLine("Недостаточно денег на балансе!"); }
                }
                public void upgrade(int current_step)
                {
                    if (balance > 7000)
                    {
                        balance -= 7000;
                        for (int i = 0; i < all_fabs.Count(); i++)
                        {
                            if (all_fabs[i] is Fabric)
                            {
                                all_fabs.Remove(all_fabs[i]);
                                break;
                            }
                        }
                        fabs_on_building.Add(new AutomatedFabric(current_step));
                        Console.WriteLine($"Вы успешно поставили фабрику на улучшение!\nВаш остаток на балансе: {balance}.");
                    }
                    else { Console.WriteLine("Недостаточно денег на балансе!"); }
                }
                public void fabric_count()
                {
                    fabrics = 0;
                    autfabrics = 0;
                    for (int i = 0; i < all_fabs.Count; i++)
                    {
                        if (all_fabs[i] is Fabric)
                        {
                            fabrics++;
                        }
                        else if (all_fabs[i] is AutomatedFabric)
                        {
                            autfabrics++;
                        }
                    }
                }
                public int payTaxes()
                {
                    int tax = tax4P * EGP + tax4M * ESM + tax4Fabric * fabrics + tax4AutomatedFabric * autfabrics;
                    balance -= tax;
                    return tax;
                }
                public void bankrot(List<Player> players, List<Player> plrsBySen)
                {
                    Console.WriteLine($"\n|-------------------------------------------------------------|");
                    Console.WriteLine($"   ): Игрок под номером {id+1} объявляется банкротом и выбывает :( ");
                    Console.WriteLine($"|-------------------------------------------------------------|\n");
                    players.RemoveAt(id);
                    for (int i = 0; i < plrsBySen.Count; i++)
                    {
                        if (!players.Contains(plrsBySen[i]))
                        {
                            plrsBySen.RemoveAt(i);
                            return;
                        }
                    }
                }
                public void show()
                {
                    line();
                    Console.WriteLine($"|  Иденфикационный номер игрока: {id + 1}\n" +
                                      $"|  Баланс игрока: {balance} $\n" +
                                      $"|  ЕСМ игрока: {ESM}\n" +
                                      $"|  ЕГП игрока: {EGP}\n" +
                                      $"|  Количество обычных фабрик у игрока: {fabrics}\n" +
                                      $"|  Количество автоматических фабрик у игрока: {autfabrics}");
                    line();
                }
                public void request4M(Bank bank, List<Player> players)
                {
                    int min = bank.Min_price4Materials();
                    double[] Mat = new double[5] { 1, 1.5, 2, 2.5, 3 };
                    int max = (int)(Mat[bank.lvl - 1] * players.Count);
                    line();
                    Console.WriteLine($"|  Минимальная цена на ЕСМ в этом месяце равняется {min} за одну единицу продукции.");
                    Console.WriteLine($"|  Максимально количество ЕСМ, которое банк может продать: {max}.");
                    Console.WriteLine("|  Введите предлагаемую вами цену на ЕСМ\n|  (ввод цены меньше, предлогаемой банком, или больше, чем есть у вас на балансе, будет считаться за отказ).");
                    line();
                    int price;
                    string str = Console.ReadLine();
                    bool t = Int32.TryParse(str, out price);
                    if (t && price > min && price < balance)
                    {
                        int count;
                        Console.WriteLine("Введите количество ЕСМ, которое хотели бы приобрести :");
                        while (true)
                        {
                            count = Int32.Parse(Console.ReadLine());
                            if (count > max || count < 0)
                                Console.WriteLine("\tВы ввели некорректное число!");
                            else
                                break;
                        }
                        if (count * price > balance)
                        {
                            Console.WriteLine($"\tВы хотите приобрести ЕСМ на сумму {count * price}, что больше вашего баланса ({balance}), следовательно, вы отказываетесь от участия.");
                            return;
                        }
                        bank.req4Mat(count, price, id);
                    }
                    else
                    {
                        Console.WriteLine("\tВы отказались от участия.\n");
                        return;
                    }
                }
                public void EGPsell(Bank bank, List<Player> players)
                {
                    if (EGP == 0)
                    {
                        Console.WriteLine("У вас недостаточно ЕГП, чтобы учавствовать в торгах\nПоднакопите ЕГП и возвращайтесь в следующем месяце");
                        return;
                    }
                    int max_price = bank.Max_price4Products_recieve();
                    double[] Mat = new double[5] {3,2.5,2,1.5,1 };
                    int max4sell = (int)(Mat[bank.lvl - 1] * players.Count);
                    line();
                    Console.WriteLine($"|  Максимальная цена на ЕГП в этом месяце равняется {max_price} за одну единицу продукции.");
                    Console.WriteLine($"|  Максимальное количество ЕГП, которое банк может купить: {max4sell}.");
                    Console.WriteLine( "|  Введите предлагаемую вами цену на ЕГП\n|  (ввод цены выше, предлогаемой банком, или нуля будет считаться за отказ).");
                    line();
                    int price;
                    bool t= Int32.TryParse(Console.ReadLine(), out price);
                    int count;
                    if (!t||price > max_price|| price==0)
                    {
                        Console.WriteLine("\tВы отказались от участия.");
                        return;
                    }
                    Console.WriteLine("Введите количество ЕГП, которое хотели бы продать: ");
                    while (true)
                    {
                        count = Int32.Parse(Console.ReadLine());
                        if (count > max4sell || count < 0 || count >EGP)
                        {
                            Console.WriteLine("\tВы ввели некорректное число!");
                        }
                        else
                            break;
                    }
                    bank.req4Prod(count, price, id);
                }
                public bool any_Fabrics()
                {
                    for (int i = 0; i < all_fabs.Count(); i++)
                    {
                        if (all_fabs[i] is Fabric)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                public bool any_AutomatedFabrics()
                {
                    for (int i = 0; i < all_fabs.Count(); i++)
                    {
                        if (all_fabs[i] is Fabric)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                public int findFab()
                {
                    for (int i = 0; i < all_fabs.Count(); i++)
                    {
                        if (all_fabs[i] is Fabric)
                        {
                            return i;
                        }
                    }
                    return 10;
                }
                public int findAutFab()
                {
                    for (int i = 0; i < all_fabs.Count(); i++)
                    {
                        if (all_fabs[i] is AutomatedFabric)
                        {
                            return i;
                        }
                    }
                    return 10;
                }
                public void take_loan(int step)
                {
                    Console.WriteLine("Введите 1 если вы хотите взять ссуду или любое другое число если не хотите: ");
                    int choice;
                    string str = Console.ReadLine();
                    bool t = Int32.TryParse(str, out choice);
                    if (t)
                    {
                        if (choice == 1)
                        {
                            int max_loan = (fabrics + autfabrics * 2) * 5000;
                            int loan = 0;
                            for (int i = 0; i < loans.Count; i++)
                                loan += loans[i][0];
                            if (max_loan - loan > max_loan / 2)
                            {
                                Console.WriteLine("Выберите какую фабрику хотите заложить: ");
                                if (any_Fabrics())
                                    Console.WriteLine("1) Обычную фабрику (получите на свой счет 5000) (Введите 1)");
                                if (any_AutomatedFabrics())
                                    Console.WriteLine("2) Автоматизированную фабрику (получите на свой счет 10000) (Введите 2)");
                                Console.WriteLine("Если вы передумали брать ссуду, введите что-либо другое");
                                int[] l = new int[3];
                                str = Console.ReadLine();
                                t = Int32.TryParse(str, out choice);
                                choice = Int32.Parse(str);
                                if ( t==true&&(choice == 1 || choice == 2))
                                {
                                    if (choice == 1)
                                    {
                                        if (findFab() != 10)
                                        {
                                            balance += 5000;
                                            l[0] = 5000;
                                            l[1] = step + 12;
                                            l[2] = 1;
                                            all_fabs.RemoveAt(findFab());
                                            Console.WriteLine("Вы заложили одну обычную фабрику и получили 5000 на баланс");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Мы не нашли у вас обычных фабрик :\\\n");
                                            return;
                                        }
                                    }
                                    else if (choice == 2)
                                    {
                                        if (findAutFab() != 10)
                                        {
                                            balance += 10000;
                                            l[0] = 10000;
                                            l[1] = step + 12;
                                            l[2] = 2;
                                            all_fabs.RemoveAt(findAutFab());
                                            Console.WriteLine("Вы заложили одну автоматизированную фабрику и получили 10000 на баланс");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Мы не нашли у вас автоматизированных фабрик :\\\n");
                                            return;
                                        }
                                    }
                                }
                                else { Console.WriteLine("Вы отказались"); return; }
                                loans.Add(l);
                            }
                            else
                            {
                                Console.WriteLine("Вы не можете взять ссуду!");
                                return;
                            }
                        }
                        else return;
                    }
                    else 
                    { Console.WriteLine("Вы отказались");
                        return;}
                }
                public void loans_check(int step)
                {
                    for (int i = 0; i < loans.Count; i++)
                    {
                        if (loans[i][1]==step)
                        {
                            Console.WriteLine("\tПришло время возвращать долг.");
                            balance -= loans[i][0];
                            Console.WriteLine($"С вас списали {loans[i][0]} и ваш баланс составил {balance}.");
                            if (loans[i][2] == 1)
                            {
                                Console.WriteLine($"Вам вернули 1 обычную фабрику.");
                                all_fabs.Add(new Fabric());
                                loans.RemoveAt(i);
                            }
                            else
                            {
                                Console.WriteLine($"Вам вернули 1 автоматизированную фабрику.");
                                all_fabs.Add(new Fabric());
                                loans.RemoveAt(i);
                            }

                        }
                    }
                }
                public void loan_tax()
                {
                    if (loans.Count != 0)
                    {
                        int tax = 0;
                        for (int i = 0; i < loans.Count; i++)
                        {
                            tax += (int)(loans[i][0] * 0.01);
                        }
                        balance -= tax;
                        Console.WriteLine($"За все имеющиеся у вас ссуды с вас списали ссудный процент ({tax}) долларов.");
                        Console.WriteLine($"Баланс после списания: {balance} долларов.");
                    }
                }
                public int max_EGP()
                {
                    int max = 0;
                    for (int i = 0; i < all_fabs.Count; i++)
                    {
                        if (all_fabs[i] is Fabric)
                            max++;
                        if (all_fabs[i] is AutomatedFabric)
                            max+=2;
                    }
                    return max;
                }
                public int minProdPrice(int ESM)
                {
                    int aFabCount = 0;
                    if (ESM < findAutFab() * 2)
                    {
                        aFabCount = ESM / 2;
                        if (aFabCount * 2 == ESM - 1)
                        {
                            return aFabCount * 3000 + 2000;
                        }
                        else
                        {
                            return aFabCount * 3000;
                        }
                    }
                    else
                    {
                        return (ESM - findAutFab() * 2) * 2000 + 3000 * findAutFab();
                    }
                }
                public void produce()
                {
                    line();
                    Console.WriteLine($"|У вас сейчас {ESM} ЕСМ и {EGP} ЕГП.");
                    Console.WriteLine($"|Если у вас есть желание произвести ЕГП, введите 1.");
                    Console.WriteLine($"|Если вы не хотите ничего производить, введите любое другое число.");
                    string str = Console.ReadLine();
                    int choice;
                    bool t = Int32.TryParse(str, out choice);
                    if (t)
                    {
                        if (choice == 1)
                        {
                            int inESM = 0;
                            Console.WriteLine($"Благодаря вашим фабрикам вы можете переработать {max_EGP()} ЕСМ.");
                            while (true)
                            {
                                Console.WriteLine($"Введите сколько ЕГП вы хотите произвести:");
                                str = Console.ReadLine();
                                t = Int32.TryParse(str,out inESM);
                                if (t)
                                {
                                    if (inESM > ESM)
                                        Console.WriteLine("У вас недостаточно ЕСМ для переработки!");
                                    else if (inESM > max_EGP())
                                        Console.WriteLine("У вас недостаточно фабрик для переработки!");
                                    else if (minProdPrice(inESM) > balance)
                                        Console.WriteLine("У вас недостаточно денег для переработки!");
                                    else
                                    {
                                        balance -= minProdPrice(inESM);
                                        ESM -= inESM;
                                        EGP += inESM;
                                        Console.WriteLine($"После переработки у игрока {id+1} имеется {ESM} ЕСМ и {EGP} ЕГП, а его баланс составляет {balance}");
                                        break;
                                    }
                                }
                                else Console.WriteLine("Введены неверные данные");
                            }
                        }
                        else  Console.WriteLine("Вы отказались");
                    }
                    else Console.WriteLine("Вы отказались");
                }
                public void building(int step)
                {
                    if (all_fabs.Count() < 6 && balance > 5000)
                    {
                        Console.WriteLine($"\nВаш баланс составляет {balance} и у вас есть места под фабрики.");
                        Console.WriteLine($"Если у вас есть желание постороить фабрику, введите 1.");
                        Console.WriteLine($"Если у вас есть желание постороить автоматизированную фабрику, введите 2.");
                        Console.WriteLine($"Если вы не хотите что-то строить, введите любое другое число.");
                        int choice;
                        bool t = Int32.TryParse(Console.ReadLine(), out choice);
                        if (t&&choice == 1)
                            buildUsual(step);
                        else if (t&&choice == 2)
                            buildAutomated(step);
                    }
                    else if (all_fabs.Count() == 6 && balance > 7000 && any_Fabrics())
                    {
                        Console.WriteLine($"\nВаш баланс составляет {balance} и у вас есть обычные фабрики, одну из которых вы можете улучшить до автоматических за 7000");
                        Console.WriteLine($"Если у вас есть желание улучшить фабрику, введите 1");
                        Console.WriteLine($"Если у вас нет такого желания, введите любое друго число");
                        int choice = Int32.Parse(Console.ReadLine());
                        bool t = Int32.TryParse(Console.ReadLine(), out choice);
                        if (t&&choice == 1)
                            upgrade(step);
                    }
                }
            };
            public class Bank
            {
                public int lvl;
                int[] price4M = new int[5] { 800,650,500,400,300 };
                int[] price4P = new int[5] { 6500,6000,5500,5000,4500};
                public void new_level_recieve()
                {
                    Random rnd = new Random();
                    int[,] levels = new int[5, 12] {
                        { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 4, 5 },
                        { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 5 },
                        { 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5 },
                        { 1, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5 },
                        { 1, 2, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5 } };
                    int rand = rnd.Next(12);
                    lvl = levels[(lvl - 1), rand];
                    return;
                }
                public int Min_price4Materials()
                {
                    int price = price4M[lvl - 1];
                    return price;
                }
                public int Max_price4Products_recieve()
                {
                    int price = price4P[lvl - 1];
                    return price;
                }
                public List<int> requestMat = new List<int>();
                public List<int> requestProd = new List<int>();
                public Bank()
                {
                    Console.WriteLine($"Создан банк.");
                    lvl = 3;

                }

                public void req4Mat(int count, int price, int id)
                {
                    requestMat.Add(count);
                    requestMat.Add(price);
                    requestMat.Add(id);
                } 
                public void req4Prod(int count, int price, int id)
                {
                    requestProd.Add(count);
                    requestProd.Add(price);
                    requestProd.Add(id);
                } 
                public void giveOut(List<Player> players)
                {
                    double[] Mat = new double[5] { 1, 1.5, 2, 2.5, 3 };
                    int count = (int)(Mat[lvl - 1] * players.Count);
                    int maxid;
                    while (count >= 1 && requestMat.Count>=3)
                    {
                        maxid = 1;
                        for (int i = 1; i < requestMat.Count; i += 3)
                        {
                            if (requestMat[i] > requestMat[maxid])
                            {
                               maxid=i;
                            }
                        }
                        if (requestMat[maxid - 1] <= count)
                        {
                           
                            players[requestMat[maxid + 1]].balance -= requestMat[maxid]* requestMat[maxid-1];
                            line();
                            Console.WriteLine($"| Игрок с индефикационным номером {players[requestMat[maxid + 1]].id} покупает {requestMat[maxid-1]} ЕСМ за {requestMat[maxid] * requestMat[maxid-1]}.");
                            Console.WriteLine($"| Баланс Игрока {players[requestMat[maxid + 1]].id} после покупки составил {players[requestMat[maxid + 1]].balance}.");
                            players[requestMat[maxid + 1]].ESM += count;
                            line();
                            count -= requestMat[maxid - 1];
                            requestMat.RemoveAt(maxid-1);
                            requestMat.RemoveAt(maxid-1);
                            requestMat.RemoveAt(maxid-1);
                        }
                        else
                        {
                            requestMat[maxid - 1]=count;
                            players[requestMat[maxid +1]].balance -= requestMat[maxid] * requestMat[maxid - 1];
                            line();
                            Console.WriteLine($"| Игрок с индефикационным номером {players[requestMat[maxid +1]].id} покупает {count} ЕСМ за {requestMat[maxid] * count}.");
                            Console.WriteLine($"| Баланс Игрока {players[requestMat[maxid + 1]].id} после покупки составил {players[requestMat[maxid + 1]].balance}.");
                            players[requestMat[maxid + 1]].ESM += count;
                            line();
                            return;
                        }
                        Console.WriteLine($"После продажи осталось {count} ЕСМ.");
                    }
                }
                public void EGPbuy(List<Player> players)
                {
                    if (requestProd.Count == 0)
                    {
                        Console.WriteLine("В этом месяце нет заявок на продажу ЕГП");
                        return;
                    }    
                    double[] price = new double[5] { 3,2.5,2,1.5,1};
                    int count = (int)(price[lvl - 1] * players.Count);
                    int min;
                    while(requestProd.Count >= 3&&count>=1)
                    {
                        min = 1;
                        for (int j = 1; j < requestProd.Count; j += 3)
                        {
                            if (requestProd[j] < requestProd[min])
                                min = j;
                        }
                        if (requestProd[min - 1] <= count)
                        {
                            players[requestProd[min + 1]].balance += requestProd[min] * requestProd[min - 1];
                            line();
                            Console.WriteLine($"| Банк купил {requestProd[min - 1]} ЕГП у игрока с индефикационным номером {players[requestProd[min + 1]].id} на сумму {requestProd[min] * requestProd[min - 1]}.");
                            Console.WriteLine($"| Баланс Игрока {players[requestProd[min + 1]].id} после покупки составил {players[requestProd[min + 1]].balance}.");
                            line();
                            count -= requestProd[min - 1];
                            players[requestProd[min + 1]].EGP = 0;
                            requestProd.RemoveAt(min);
                            requestProd.RemoveAt(min);
                            requestProd.RemoveAt(min - 1);
                        }
                        else
                        {
                            requestProd[min - 1] = count;
                            players[requestProd[min + 1]].balance += requestProd[min] * requestProd[min - 1];
                            line();
                            Console.WriteLine($"| Банк купил {requestProd[min - 1]} ЕГП у игрока с индефикационным номером {players[requestProd[min + 1]].id} на сумму {requestProd[min] * requestProd[min - 1]}.");
                            Console.WriteLine($"| Баланс Игрока {players[requestProd[min + 1]].id} после покупки составил {players[requestProd[min + 1]].balance}.");
                            players[requestProd[min + 1]].EGP -= count;
                            line();
                            return;
                        }
                        if (count > 0)
                            Console.WriteLine($"Банк готов купить еще {count} ЕГП.");
                        else
                            Console.WriteLine($"Банк больше не может покупать продукцию в этом месяце");
                    }
                }
            };
            static public void winner(List<Player> players, Bank bank)
            {
                int bal, max_bal;
                max_bal = (players[0].fabrics * 5000) + (players[0].autfabrics * 10000) + (bank.Min_price4Materials() * players[0].ESM) + (bank.Max_price4Products_recieve() * players[0].EGP) + players[0].balance;
                int id = 0;
                if (players.Count == 1)
                {
                    id = players[0].id;
                }
                else
                {
                    foreach (Player player in players)
                    {
                        bal = (player.fabrics * 5000) + (player.autfabrics * 10000) + (bank.Min_price4Materials() * player.ESM) + (bank.Max_price4Products_recieve() * player.EGP) + player.balance;
                        if (bal > max_bal)
                        {
                            max_bal = bal;
                            id = player.id;
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine($"|--------------------------------------------------------------------------------------------------------------------|");
                Console.WriteLine($"|                                                                                                                    |");
                Console.WriteLine($"|    Победителем объявляется игрок с индефикационным номером {id}! Этот игрок набрал {max_bal} баллов по окончанию игры     |");
                Console.WriteLine($"|                                                   Поздравялем!!!                                                   |");
                Console.WriteLine($"|                                                                                                                    |");
                Console.WriteLine($"|--------------------------------------------------------------------------------------------------------------------|");  
            }
            public List<Player> playersBySeniority(List<Player> players, ref int mainPlrID)
            {

                List<Player> lst = new List<Player>(6);
                int length = players.Count;

                if (mainPlrID > length - 1)
                {
                    mainPlrID = 0;
                }
                for (int i = mainPlrID; i < length; i++)
                {
                    lst.Add(players[i]);

                }
                if (mainPlrID != 0)
                {
                    for (int i = 0; i < mainPlrID; i++)
                    {

                        lst.Add(players[i]);
                    }
                }

                return lst;
            }
            static public void confirm()
            {
                Console.WriteLine("Нажмите ENTER, чтобы продолжить");
                Console.ReadLine();
                Console.Clear();
            }
            static public void line()
            { Console.WriteLine($" ---------------------------------------------------------------------------------");}
            static void step(List<Player> players, List<Player> plrsBySen, Bank bank, ref int mainPlrID, int step)
            {
                bank.requestMat.Clear();
                bank.requestProd.Clear();
                if (step != 0) 
                bank.new_level_recieve();
                for (int i = 0; i < plrsBySen.Count(); i++)
                {
                    Console.WriteLine($"\n\t\t\tСейчас месяц под номером {step+1}.");
                    Console.WriteLine($"\t\t\tУровень банка в этом месяце {bank.lvl}.\n");
                    plrsBySen[i].fabric_count();
                    plrsBySen[i].loan_tax();
                    plrsBySen[i].loans_check(step);
                    foreach (IFab fab in plrsBySen[i].fabs_on_building)
                    {
                        fab.build(plrsBySen[i],step);
                    }
                    if (i <= players.Count())
                    {
                        line();
                        Console.WriteLine($"|  Баланс игрока {plrsBySen[i].id+1} до уплаты налогов: {plrsBySen[i].balance}.");
                        Console.WriteLine($"|  Ежемесячные издержки игрока под номером {plrsBySen[i].id + 1} составил: {plrsBySen[i].payTaxes()}.");
                        line();
                        if (plrsBySen[i].balance <= 0)
                        {
                            plrsBySen[i].bankrot(players, plrsBySen);
                        }
                        else
                        {
                           plrsBySen[i].show();
                           plrsBySen[i].take_loan(step);
                           plrsBySen[i].request4M(bank, players);
                            if (plrsBySen[i].balance > 2000 && plrsBySen[i].ESM >= 1)
                            {
                                plrsBySen[i].produce();
                            }
                            plrsBySen[i].EGPsell(bank, players);
                            plrsBySen[i].building(step);
                        }
                    }
                    else
                    {
                        winner(players,bank);
                        return;
                    }
                }
                bank.giveOut(players);
                bank.EGPbuy(players);
                Console.WriteLine(" ------------ ");
                Console.WriteLine("|Совершен ход|");
                Console.WriteLine(" ------------ ");
                confirm();
                return;
            }
            public int in_step()
            {
                string str;
                int steps;
                bool t = false;
                Console.Clear();
                Console.WriteLine("Введите количество ходов (от 5 до 42 ходов):");
                str = Console.ReadLine();
                t = Int32.TryParse(str, out steps);
                if (t)
                {
                    steps = Int32.Parse(str);
                    if (steps >= 5 && steps <= 42)
                    {
                        return steps;
                    }
                    else
                    {
                        return steps = in_step();
                    }
                }
                else
                {
                    return steps = in_step();
                }
            }
            public int in_count()
            {
                string str;
                int player_count;
                bool t = false;
                Console.Clear();
                Console.WriteLine("Введите количество игроков (от 2 до 6 игроков):");
                str = Console.ReadLine();
                t = Int32.TryParse(str, out player_count);
                if (t)
                {
                    player_count = Int32.Parse(str);
                    if (player_count >= 2 && player_count <= 6)
                    {
                        return player_count;
                    }
                    else
                    {
                        return player_count = in_count();
                    }
                }
                else
                {
                    return player_count = in_count();
                }
            }
            public bool start_frame()
            {
                Console.WriteLine(" ___________________________________________________________________________________________________________\n" +
                                  "|                                                                                                           |");
                Console.WriteLine("|\t\t\t\t   ||    //|   <======   <======     || \t\t\t\t    |\n" +
                                  "|\t\t\t\t   ||   //||   ||       ||    ||    //\\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   ||  // ||   ||       ||    ||   //  \\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   || //  ||   ||       ||======  //====\\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   ||//   ||   ||       ||       //      \\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   |//    ||   ||       ||      //        \\\\ \t\t\t\t    |");
                Console.WriteLine("|                                                                                                           |");
                Console.WriteLine("|  |\\\\        //|      ||=====      ||     ||     ||=====         ||        \\\\ || //    ||=====    <======  |\n" +
                                  "|  ||\\\\      //||      ||           ||     ||     ||             //\\\\        \\\\||//     ||        ||    ||  |\n" +
                                  "|  || \\\\    // ||      ||=====      ||=====||     ||=====       //  \\\\        \\\\//      ||=====   ||    ||  |\n" +
                                  "|  ||  \\\\  //  ||      ||           ||     ||     ||           //    \\\\        \\\\       ||        ||======  |\n" +
                                  "|  ||   \\\\//   ||      ||           ||     ||     ||         ============     //\\\\      ||        ||\t    |\n" +
                                  "|  ||    ||    ||      ||=====      ||     ||     ||=====     ||      ||     //||\\\\     ||=====   ||\t    |");
                Console.WriteLine("|___________________________________________________________________________________________________________|\n");
                Console.WriteLine("\t\t\t\t\t\t-> Играть\n" +"\t\t\t\t\t\t  Выйти");
                return false;
            }
            public bool start_frame2()
            {
                Console.WriteLine(" ___________________________________________________________________________________________________________\n" +
                                  "|                                                                                                           |");
                Console.WriteLine("|\t\t\t\t   ||    //|   <======   <======     || \t\t\t\t    |\n" +
                                  "|\t\t\t\t   ||   //||   ||       ||    ||    //\\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   ||  // ||   ||       ||    ||   //  \\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   || //  ||   ||       ||======  //====\\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   ||//   ||   ||       ||       //      \\\\ \t\t\t\t    |\n" +
                                  "|\t\t\t\t   |//    ||   ||       ||      //        \\\\ \t\t\t\t    |");
                Console.WriteLine("|                                                                                                           |");
                Console.WriteLine("|  |\\\\        //|      ||=====      ||     ||     ||=====         ||        \\\\ || //    ||=====    <======  |\n" +
                                  "|  ||\\\\      //||      ||           ||     ||     ||             //\\\\        \\\\||//     ||        ||    ||  |\n" +
                                  "|  || \\\\    // ||      ||=====      ||=====||     ||=====       //  \\\\        \\\\//      ||=====   ||    ||  |\n" +
                                  "|  ||  \\\\  //  ||      ||           ||     ||     ||           //    \\\\        \\\\       ||        ||======  |\n" +
                                  "|  ||   \\\\//   ||      ||           ||     ||     ||         ============     //\\\\      ||        ||\t    |\n" +
                                  "|  ||    ||    ||      ||=====      ||     ||     ||=====     ||      ||     //||\\\\     ||=====   ||\t    |");
                Console.WriteLine("|___________________________________________________________________________________________________________|\n");
                Console.WriteLine("\t\t\t\t\t\t  Играть\n" + "\t\t\t\t\t\t-> Выйти");
                return true;
            }
            public void game() //функция всей игры
            {  
                bool exit = false;
                ConsoleKey key;
                start_frame();
                while (true)
                {
                    key = Console.ReadKey(true).Key;
                    Console.Clear();
                    if (key == ConsoleKey.UpArrow || key.ToString() == "W")
                    {
                        start_frame();
                        exit = false;
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        start_frame2();
                        exit = true;
                    }
                    else if (exit == true)
                    {
                        start_frame2();
                    }
                    else if (exit == false)
                    {
                        exit = false;
                        start_frame();
                    }
                    if (key == ConsoleKey.Enter && exit == true)
                    {
                        Console.Clear();
                        return;
                    }
                    if (key == ConsoleKey.Enter && exit == false)
                    {
                        break;
                    }

                }
                Random rnd = new Random();
                Bank bank = new Bank();
                int steps = in_step(), player_count = in_count();
                List<Player> players = new List<Player>();
                List<Player> plrsBySen = new List<Player>();
                Player p0 = new Player(0);
                Player p1 = new Player(1);
                Player p2 = new Player(2);
                Player p3 = new Player(3);
                Player p4 = new Player(4);
                Player p5 = new Player(5);
                for (int i = 0; i < player_count; i++)
                {
                    players.Add(new Player(i));
                }
                Console.Clear();
                Console.WriteLine(" ---------------------------- ");
                Console.WriteLine($"| *_* Создано {players.Count} игроков. *_* |");//krasivo
                Console.WriteLine(" ---------------------------- ");
                int mainPlrID = 0;
                confirm();
                for (int i = 0; i < steps; i++)
                {
                    plrsBySen = playersBySeniority(players, ref mainPlrID);
                    step(players, plrsBySen, bank, ref mainPlrID,i);
                    steps--;
                    mainPlrID++;
                    confirm();
                    if (players.Count() <= 1)
                    {
                        winner(players, bank);
                        break;
                    }
                }
            }
            static void Main(string[] args) 
            {
                All_game game = new All_game();
                game.game();


            }
        }
    }
}