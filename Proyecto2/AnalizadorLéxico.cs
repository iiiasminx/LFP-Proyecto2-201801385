﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto2
{
    class AnalizadorLéxico
    {
        private LinkedList<TokenC> Salida;
        private int estado;
        private String auxlex, comentario;

        int contador;

        int contaerror = 1;
        int contafila = 1;
        int contacolumna = 1;
        String descerror = "caracter desconocido";

        int contatoken = 1;

        public LinkedList<ErrorLéxico> ListaDeErrores = new LinkedList<ErrorLéxico>();
        public LinkedList<ObjToken> ListaTokens = new LinkedList<ObjToken>();

        //BANDERAS 
        Boolean prabierta = true;
        Boolean nvarabierta = false;
        Boolean escadena = false;
        Boolean eschar = false;
        Boolean writelineabierto = false, dentroparentesiswriteline = false;

        int ya_punto = 0;

        ObjToken[] arreglotokens;

        public LinkedList<TokenC> escanear(String entrada)
        {
            entrada = entrada + '%';
            Salida = new LinkedList<TokenC>();
            estado = 0;
            auxlex = "";


            Char c;
            for (int i = 0; i < entrada.Length - 1; i++)
            {
                c = entrada.ElementAt(i);

                switch (estado)
                {
                    #region el que los manda a todos lados
                    case 0:
                        if (char.IsLetter(c))
                        {
                            estado = 1;
                            auxlex += c;
                            contacolumna++;
                        }
                        else if (char.IsDigit(c))
                        {
                            estado = 4;
                            auxlex += c;

                            contador = 1;
                            contacolumna++;
                        }
                        else if (c.CompareTo(';') == 0)
                        {
                            auxlex += c;
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 1, "Punto y coma");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            contacolumna++;
                            agregarToken(TokenC.Tipo.puntoycoma);
                            nvarabierta = false;
                            prabierta = true;
                        }
                        else if (c.CompareTo(' ') == 0)
                        {
                            //auxlex += c;
                            contacolumna++;
                        }
                        else if (c.CompareTo('.') == 0)
                        {
                            auxlex += c;
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "punto");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            contacolumna++;
                            agregarToken(TokenC.Tipo.punto);
                        }
                        else if (c.CompareTo(':') == 0)
                        {
                            auxlex += c;
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "dos puntos");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            contacolumna++;
                            prabierta = true;
                            agregarToken(TokenC.Tipo.dospuntos);
                        }
                        else if (c.CompareTo('*') == 0)
                        {

                            estado = 25;
                            auxlex += c;
                            contacolumna++;

                        }
                        else if (c.CompareTo('>') == 0)
                        {
                            estado = 19;
                            auxlex += c;
                            contacolumna++;

                        }
                        else if (c.CompareTo('<') == 0)
                        {
                            estado = 21;
                            auxlex += c;
                            contacolumna++;
                        }
                        else if (c.CompareTo(',') == 0)
                        {
                            auxlex += c;
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "coma");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            contacolumna++;
                            prabierta = false;
                            nvarabierta = true;
                            agregarToken(TokenC.Tipo.coma);
                        }
                        else if (c.CompareTo('+') == 0)
                        {
                            estado = 13;
                            auxlex += c;
                            contacolumna++;

                        }
                        else if (c.CompareTo('-') == 0)
                        {
                            estado = 15;
                            auxlex += c;
                            contacolumna++;

                        }
                        else if (c.CompareTo('=') == 0)
                        {
                            estado = 6;
                            auxlex += c;
                            contacolumna++;
                        }
                        else if (c.CompareTo('!') == 0)
                        {
                            estado = 17;
                            auxlex += c;
                            contacolumna++;
                        }
                        else if (c.CompareTo('/') == 0)
                        {
                            estado = 8;
                            auxlex += c;
                            contacolumna++;
                        }
                        else if (c.CompareTo('\n') == 0 || c.Equals("\r")
                            || c.Equals("\n"))
                        {
                            auxlex = "";
                            estado = 0;
                            contafila++;
                            contacolumna = 0;
                        }
                        else if (c.CompareTo('\t') == 0 || c.Equals("\t"))
                        {
                            auxlex = "";
                            estado = 0;
                        }
                        else if (c.CompareTo('"') == 0)
                        {
                            auxlex += c;
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "Comillas dobles");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.comillas_dobles);
                            contacolumna++;
                            //false = no hay cadena abierta
                            //true = ya hay una cadena abierta
                            if (escadena == false)
                            {
                                escadena = true;
                                // aqui abro cadena
                                estado = 11;
                            }
                            else if (escadena == true)
                            {
                                escadena = false;
                                //aquí cierro cadena
                            }

                        }
                        else if (c.CompareTo('\'') == 0)
                        {
                            auxlex += c;
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "Comillas simples");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.comillas_simples);
                            contacolumna++;
                            //false = no hay cadena abierta
                            //true = ya hay una cadena abierta
                            if (eschar == false)
                            {
                                eschar = true;
                                // aqui abro cadena
                                estado = 12;
                            }
                            else if (eschar == true)
                            {
                                eschar = false;
                                //aquí cierro cadena
                            }

                        }
                        else if (c.CompareTo('{') == 0)
                        {
                            auxlex += c;

                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 14, "llave que abre");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.llave_abrir);
                            prabierta = true;
                            contacolumna++;
                        }
                        else if (c.CompareTo('}') == 0)
                        {
                            auxlex += c;

                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 14, "llave que cierra");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.llave_cerrar);
                            contacolumna++;
                            prabierta = true;
                            nvarabierta = false;
                        }
                        else if (c.CompareTo('[') == 0)
                        {
                            auxlex += c;

                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 14, "corchete que abre");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.corchete_abrir);
                            contacolumna++;
                        }
                        else if (c.CompareTo(']') == 0)
                        {

                            auxlex += c;

                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 14, "corchete que cierra");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.corchete_cerrar);
                            contacolumna++;


                        }
                        else if (c.CompareTo('(') == 0)
                        {
                            auxlex += c;

                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 14, "paréntestis que abre");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.parentesis_abrir);
                            contacolumna++;

                            if (dentroparentesiswriteline)
                            {
                                //estado = 13;
                                prabierta = true;
                            }
                        }
                        else if (c.CompareTo(')') == 0)
                        {
                            auxlex += c;

                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 14, "paréntesis que cierra");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.parentesis_cerrar);
                            contacolumna++;

                            if (dentroparentesiswriteline)
                            {
                                dentroparentesiswriteline = false;
                            }
                        }
                        else
                        {
                            if (c.CompareTo('%') == 0)
                            {
                                //Hemos concluido el análisis léxico.
                                Console.WriteLine("Hemos concluido el analiss con exito");

                            }
                            else
                            {
                                if (c.CompareTo('\n') == 0 || c.CompareTo('\t') == 0 || c.CompareTo('\r') == 0
                                    || c.Equals("\n") || c.Equals("\r") || c.Equals("\n\t") || c.Equals("\t"))
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Error lexico con: " + c);
                                    ErrorLéxico e1 = new ErrorLéxico(contaerror, contafila, contacolumna, c, descerror);
                                    ListaDeErrores.AddLast(e1);
                                    contacolumna++;
                                    estado = 0;
                                    auxlex = "";
                                }
                            }
                        }
                        break;
                    #endregion
                    #region si comienza por char
                    case 1:
                        if (Char.IsLetter(c))
                        {
                            estado = 1;
                            auxlex += c;
                            contacolumna++;
                            //así voy juntando caracteres
                        }
                        else if (Char.IsDigit(c))
                        {
                            estado = 1;
                            auxlex += c;
                            contacolumna++;
                            prabierta = false;
                            nvarabierta = true;

                        }
                        else if (c.CompareTo(' ') == 0 || c.CompareTo(';') == 0
                            || c.CompareTo('\t') == 0 || c.CompareTo('\n') == 0 || c.CompareTo(',') == 0
                            || c.CompareTo('(') == 0 || c.CompareTo(')') == 0 || c.CompareTo('[') == 0 || c.CompareTo('.') == 0 
                            || c.CompareTo('+') == 0 || c.CompareTo('-') == 0 || c.CompareTo(':') == 0)
                        {
                            contacolumna++;

                            if (c.CompareTo(',') == 0)
                            {
                                nvarabierta = true;
                                prabierta = false;
                            }

                            if (prabierta == true)
                            {
                                prabierta = false;
                                estado = 2;
                            }
                            else if (nvarabierta == true)
                            {
                                nvarabierta = false;
                                estado = 3;
                            }
                            else if (writelineabierto == true)
                            {
                                writelineabierto = false;
                                estado = 2;
                            }
                            else
                            {
                               /* MessageBox.Show("El texto ingresado contiene errores, por favor" +
                                "corrígalos e intente de nuevo :c \n" + "->" + auxlex + "<-\n" + c, "Error de Léxico ");*/
                                ErrorLéxico e1 = new ErrorLéxico(contaerror, contafila, contacolumna, c, descerror);
                                ListaDeErrores.AddLast(e1);
                                contacolumna++;
                                estado = 0;
                                auxlex = "";
                            }
                            i -= 1;
                        }
                        break;
                    #endregion
                    #region si es palabra reservada
                    case 2:
                        if (auxlex.Equals("class"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 10, "Palabra reservada - clase");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.PR_CLASS);
                            nvarabierta = true;
                            i -= 1;
                        }
                        if (auxlex.Equals("graficarVector") || auxlex.Equals("graficarvector")|| auxlex.Equals(" graficarvector"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 10, "Palabra reservada - graficar");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.graficar);
                            nvarabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("static"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - static");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.PR_STATIC);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("void") || auxlex.Equals(" void") || auxlex.Equals("void "))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "Palabra reservada - void");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.PR_VOID);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("Main") || auxlex.Equals(" Main") || auxlex.Equals("Main "))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "Palabra reservada - main");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.pr_main);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("int"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 13, "Palabra reservada - int");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.pr_int);
                            nvarabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("float"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - float");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.pr_float);
                            nvarabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("String") || auxlex.Equals("string"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - String");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.pr_string);
                            nvarabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("bool"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - bool");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.pr_bool);
                            nvarabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("false") || auxlex.Equals(" false") || auxlex.Equals("false "))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - false");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_false);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("true") || auxlex.Equals(" true") || auxlex.Equals("true "))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - true");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_true);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("char") || auxlex.Equals(" char") || auxlex.Equals("char "))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - char");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.pr_char);
                            nvarabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("new"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - new");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_new);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("Console"))
                        {
                            if (writelineabierto == false)
                            {
                                writelineabierto = true;
                                ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - console");
                                ListaTokens.AddLast(o1);
                                contatoken++;
                                agregarToken(TokenC.Tipo.console_writeline);
                                i -= 1;
                            }
                            dentroparentesiswriteline = true;

                        }
                        else if (auxlex.Equals("WriteLine") || auxlex.Equals(" WriteLine") || auxlex.Equals("WriteLine "))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - writeLine");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.console_writeline);
                            i -= 1;
                            dentroparentesiswriteline = true;
                        }
                        else if (auxlex.Equals("if"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - if");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_if);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("else"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - else");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_else);
                            i -= 1;
                        }
                        else if (auxlex.Equals("switch"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - switch");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_switch);
                            nvarabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("case"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - case");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_case);
                            i -= 1;
                        }
                        else if (auxlex.Equals("default") || auxlex.Equals("default "))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - default");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.pr_default);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("break"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - break");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_break);
                            i -= 1;
                        }
                        else if (auxlex.Equals("for"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - for");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_for);
                            prabierta = true;
                            i -= 1;
                        }
                        else if (auxlex.Equals("while"))
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "Palabra reservada - bool");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.un_while);
                            prabierta = true;
                            i -= 1;
                        }
                        else
                        {
                            /*Console.WriteLine("NO SE RECONOCE PALABRA RESERVADA");
                            */
                            Console.WriteLine(auxlex + " no es una palabra reservada");
                            prabierta = false;

                            arreglotokens = new ObjToken[ListaTokens.Count()];
                            ListaTokens.CopyTo(arreglotokens, 0);

                            for (int k = 0; k < arreglotokens.Length; k++)
                            {
                                //Console.WriteLine(auxlex + "comparando con" + arreglotokens[k].getLexema());

                                if (auxlex.Equals(" " + arreglotokens[k].getLexema()) || auxlex.Equals(arreglotokens[k].getLexema())
                                    || auxlex.Equals(" " + arreglotokens[k].getLexema() + " ") || auxlex.Equals(arreglotokens[k].getLexema() + " "))
                                {
                                    Console.WriteLine("match found");
                                    nvarabierta = true;
                                }
                                else if (arreglotokens[k].getLexema().Equals(" " + auxlex) || arreglotokens[k].getLexema().Equals(auxlex + " "))
                                {
                                    Console.WriteLine("match found");
                                    nvarabierta = true;
                                }
                            }
                            estado = 1;
                            i -= 1;
                        }
                        break;
                    #endregion
                    #region si es nombre de variable
                    case 3:
                        ObjToken o2 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 11, "nombre de algo");
                        ListaTokens.AddLast(o2);
                        contatoken++;
                        agregarToken(TokenC.Tipo.nombre_algo);
                        i -= 1;
                        nvarabierta = false;
                        prabierta = true;
                        break;
                    #endregion
                    #region si es un dígito
                    case 4:
                        if (Char.IsDigit(c))
                        {
                            estado = 4;
                            auxlex += c;
                            contacolumna++;
                            //así voy juntando digitos

                        }
                        else if (c.Equals('.'))
                        {
                            estado = 27;
                            auxlex += c;
                            contacolumna++;
                           
                        }
                        else
                        {
                                ObjToken o4 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "Número");
                                ListaTokens.AddLast(o4);
                                contatoken++;
                                agregarToken(TokenC.Tipo.numero);
                                i -= 1;
                          
                            
                        }
                        break;
                    #endregion
                    #region si es un float
                    case 5:
                        ObjToken o5 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 13, "Número float");
                        ListaTokens.AddLast(o5);
                        contatoken++;
                        agregarToken(TokenC.Tipo.float_algo);
                        i -= 1;
                        break;
                    #endregion

                    #region si es un igual
                    case 6:
                        if (c.Equals('='))
                        {
                            estado = 7;
                            auxlex += c;
                            contacolumna++;
                            //así voy juntando iguales
                        }
                        else
                        {
                            ObjToken o4 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "igual");
                            ListaTokens.AddLast(o4);
                            contatoken++;
                            agregarToken(TokenC.Tipo.igual);
                            i -= 1;
                        }
                        break;
                    #endregion
                    #region si es un igual igual
                    case 7:
                        ObjToken o6 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "Igual Igual");
                        ListaTokens.AddLast(o6);
                        contatoken++;
                        agregarToken(TokenC.Tipo.igualigual);
                        i -= 1;
                        break;
                    #endregion
                    #region si es un dividido
                    case 8:
                        if (c.Equals('/'))
                        {
                            estado = 9;
                            auxlex += c;
                            contacolumna++;
                            //así voy juntando divididos
                        }
                        else if (c.Equals('*'))
                        {
                            estado = 23;
                            auxlex += c;
                            contacolumna++;
                        }
                        else
                        {
                            ObjToken o7 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "dividido");
                            ListaTokens.AddLast(o7);
                            contatoken++;
                            agregarToken(TokenC.Tipo.dividido);
                            i -= 1;
                        }
                        break;
                    #endregion
                    #region inicio comentario
                    case 9:

                        ObjToken o8 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "com. 1 linea");
                        ListaTokens.AddLast(o8);
                        contatoken++;
                        agregarToken(TokenC.Tipo.comentario_unalinea);
                        i -= 1;
                        estado = 10;

                        break;
                    #endregion
                    #region cuerpo comentario 1 linea
                    case 10:

                        if (c.CompareTo('\n') == 0)
                        {
                            ObjToken o9 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "cuerpo com. 1 linea");
                            ListaTokens.AddLast(o9);
                            contatoken++;
                            agregarToken(TokenC.Tipo.cuerpo_com1l);
                            i -= 1;
                            contacolumna++;
                        }
                        else
                        {
                            estado = 10;
                            auxlex += c;
                        }
                        break;
                    #endregion
                    #region cadenas
                    case 11:
                        if (c.CompareTo('"') == 0)
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 6, "Cadena");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.cadena);
                            i -= 1;
                            contacolumna++;
                        }
                        else
                        {
                            estado = 11;
                            auxlex += c;
                        }
                        break;
                    #endregion
                    #region caracteres
                    case 12:
                        if (c.CompareTo('\'') == 0)
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 6, "caracter");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            agregarToken(TokenC.Tipo.caracter);
                            i -= 1;
                            contacolumna++;
                        }
                        else
                        {
                            estado = 12;
                            auxlex += c;
                        }
                        break;
                    #endregion
                    #region si comienza por mas
                    case 13:
                        if (c.Equals('+'))
                        {
                            estado = 14;
                            auxlex += c;
                            contacolumna++;
                            //así voy juntando mases
                        }
                        else
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "signo mas");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            contacolumna++;
                            agregarToken(TokenC.Tipo.mas);
                            i -= 1;
                        }

                        break;
                    case 14:

                        ObjToken o15 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "mas mas");
                        ListaTokens.AddLast(o15);
                        contatoken++;
                        agregarToken(TokenC.Tipo.masmas);
                        i -= 1;

                        break;
                    #endregion
                    #region si comienza por menos
                    case 15:
                        if (c.Equals('-'))
                        {
                            estado = 16;
                            auxlex += c;
                            contacolumna++;
                            //así voy juntando menoss
                        }
                        else
                        {
                            ObjToken o1 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "signo menos");
                            ListaTokens.AddLast(o1);
                            contatoken++;
                            contacolumna++;
                            agregarToken(TokenC.Tipo.menos);
                            i -= 1;
                        }

                        break;
                    case 16:

                        ObjToken o16 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "menos menos");
                        ListaTokens.AddLast(o16);
                        contatoken++;
                        agregarToken(TokenC.Tipo.menosmenos);
                        i -= 1;

                        break;
                    #endregion
                    #region Si es !=
                    case 17:
                        if (c.Equals('='))
                        {
                            estado = 18;
                            auxlex += c;
                            contacolumna++;
                        }
                        else
                        {
                            MessageBox.Show("Tienes un error en uno de tus operadores :C");
                        }
                        break;
                    case 18:
                        ObjToken o17 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "diferente a");
                        ListaTokens.AddLast(o17);
                        contatoken++;
                        agregarToken(TokenC.Tipo.diferente_que);
                        i -= 1;
                        break;

                    #endregion
                    #region >
                    case 19:
                        if (c.Equals('='))
                        {
                            estado = 20;
                            auxlex += c;
                            contacolumna++;
                        }
                        else
                        {
                            auxlex += c;
                            ObjToken o18 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "mayor que");
                            ListaTokens.AddLast(o18);
                            contatoken++;
                            contacolumna++;
                            agregarToken(TokenC.Tipo.mayor_que);
                        }                        
                        break;
                    case 20:
                        auxlex += c;
                        ObjToken o20 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "mayor o igual");
                        ListaTokens.AddLast(o20);
                        contatoken++;
                        contacolumna++;
                        agregarToken(TokenC.Tipo.mayor_igual);
                        break;
                    #endregion
                    #region <
                    case 21:
                        if (c.Equals('='))
                        {
                            estado = 22;
                            auxlex += c;
                            contacolumna++;
                        }
                        else
                        {
                            auxlex += c;
                            ObjToken o18 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "menor que");
                            ListaTokens.AddLast(o18);
                            contatoken++;
                            contacolumna++;
                            agregarToken(TokenC.Tipo.menor_que);
                        }
                        break;
                    case 22:
                        auxlex += c;
                        ObjToken o19 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "menor o igual");
                        ListaTokens.AddLast(o19);
                        contatoken++;
                        contacolumna++;
                        agregarToken(TokenC.Tipo.menor_igual);
                        break;

                    #endregion
                    #region comentario multilinea
                    case 23:

                        ObjToken o21 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "cmultilinea_abrir");
                        ListaTokens.AddLast(o21);
                        contatoken++;
                        agregarToken(TokenC.Tipo.cmultilinea_abrir);
                        //i -= 1;
                        estado = 24;

                        break;
                    case 24:

                        if (c.CompareTo('*') == 0)
                        {

                            ObjToken o9 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "cuerpo com. multilinea");
                            ListaTokens.AddLast(o9);
                            contatoken++;
                            agregarToken(TokenC.Tipo.cuerpo_comml);
                            contacolumna++;
                            i -= 1;

                        }
                        else
                        {
                            auxlex += c;
                        }

                        break; 
                    case 25:

                        if (c.Equals('/'))
                        {
                            estado = 26;
                            auxlex += c;
                            contacolumna++;
                        }
                        else
                        {
                            auxlex += c;
                            ObjToken o22 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 3, "por");
                            ListaTokens.AddLast(o22);
                            contatoken++;
                            contacolumna++;
                            prabierta = true;
                            agregarToken(TokenC.Tipo.por);
                        }                        

                        break;
                    case 26:
                        ObjToken o23 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "cmultilinea_cerrar");
                        ListaTokens.AddLast(o23);
                        contatoken++;
                        agregarToken(TokenC.Tipo.cmultilinea_cerrar);
                        break;
                    #endregion
                    case 27:
                        if (Char.IsDigit(c))
                        {
                            estado = 27;
                            auxlex += c;
                            contacolumna++;
                            //así voy juntando digitos

                        }
                        else
                        {
                            ObjToken o4 = new ObjToken(contatoken, contafila, contacolumna, auxlex, 12, "Número decimal");
                            ListaTokens.AddLast(o4);
                            contatoken++;
                            agregarToken(TokenC.Tipo.float_algo);
                            i -= 1;

                        }


                        break;
                }

            }
            return Salida;
        }

        public void agregarToken(TokenC.Tipo tipo)
        {
            Salida.AddLast(new TokenC(tipo, auxlex));
            auxlex = "";
            estado = 0;
        }

        public void imprimirListaTokens(LinkedList<TokenC> lista)
        {
            foreach (TokenC item in lista)
            {
                Console.WriteLine(item.getTipo() + ": " + item.getValorToken());
            }
        }

    }

}
