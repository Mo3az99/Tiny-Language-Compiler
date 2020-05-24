using Analyzer;
using dParser;
using Scanner;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;


namespace dParser
{
   
    public partial class Form1 : Form
    {
        Token scanner = new Token();

        Parser parser;
       
        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";

        }

        public void button1_Click(object sender, EventArgs e)
        {
            var str = richTextBox1.Text;

            
            scanner.getToken(str);
            int sca =scanner.getsc();
            richTextBox2.Text = scanner.slicer(scanner.GetText());

            parser = new Parser(scanner.getTokenQueue());
            parser.gettree(ref treeView1);

            parser.parse(sca);
            


        }
        public void button3_Click(object sender, EventArgs e)
        {
            this.treeView1.BringToFront();

        
        }


   
    }
}
class Parser :dParser.Form1
{
    //Form1 ff = new Form1();
    
   //Token scann = new Token();
    
    int i = -1, j = -1, k = -1 , h = -1;
    string addop = "";
    string term1 = "";
    string term2 = "";
    string comp = "";
    int sc;
    Queue<TokenRec> tokenQueue = new Queue<TokenRec>();

    public Parser(Queue<TokenRec> tokenQueue)
    {
        this.tokenQueue = tokenQueue;
    }
    public void parse(int sca)
    {
        //int sc;
        this.sc = sca;

       string s = "Program ";
       test.Nodes.Add(s);
        //test.Nodes[0].Nodes.Add("1");
        //test.Nodes[0].Nodes[0].Nodes.Add("2");
        //test.Nodes[0].Nodes[0].Nodes[0].Nodes.Add("3");
        //test.Nodes[0].Nodes[0].Nodes[0].Nodes[0].Nodes.Add("4");
      
    
        runProgram();
      
    }

    public TokenRec runProgram()
    {

        return stmtSequence();
    }

    private TokenRec stmtSequence()
    {
      //  while (tokenQueue.Count != 0)
      //  {
            TokenRec currentToken = tokenQueue.Dequeue();
            Console.WriteLine(currentToken.Token_Type);
            currentToken = statement(currentToken);
            while (sc > 1){
                while (tokenQueue.Count != 0 && currentToken.Token_Type == TokenRec.TokenType.SEMICOLON)
                {
                    Match(TokenRec.TokenType.SEMICOLON, currentToken);

                    // string sc = currentToken.getTokenValue();
                    //i = 0;
                    j = -1;
                    k = -1;
                    h = -1;

                    //  int sc = scanner.getsc();

                    currentToken = tokenQueue.Dequeue();
                    currentToken = statement(currentToken);
                 sc--;
                }
            }
             //sc--;
           /*  while (sc > 1)
               {
                currentToken = statement(currentToken);
                currentToken = tokenQueue.Dequeue();
                }
                */
        if (tokenQueue.Count != 0)
            {
                if (currentToken.Token_Type == TokenRec.TokenType.ELSE)
                {
                    return currentToken;
                }
                else if (currentToken.Token_Type == TokenRec.TokenType.UNTIL)
                {
                    Console.WriteLine(currentToken.getTokenValue());
                    return currentToken;
                }
                else
                {
                    Console.WriteLine(currentToken.getTokenValue());
                    return statement(currentToken);
                }
            }

            else
            {
                Console.WriteLine(currentToken.getTokenValue());
                return currentToken;
            }
     //   }
    }
    TokenRec statement(TokenRec currentToken)
    {

        switch (currentToken.Token_Type)
        {
            case TokenRec.TokenType.IDENTIFIER:
                return assignStmt(currentToken);

            case TokenRec.TokenType.IF:
                return ifStmt(currentToken);

            case TokenRec.TokenType.READ:
                return readStmt(currentToken);

            case TokenRec.TokenType.REPEAT:
                return repeatStmt(currentToken);

            case TokenRec.TokenType.WRITE:
                return writeStmt(currentToken);


            default:
                Console.WriteLine(currentToken.getTokenValue());
                throw new SystemException("Invalid statement found.");

        }


    }

    private TokenRec readStmt(TokenRec currentToken)
    {
        string rd;
        rd = currentToken.getTokenValue();
        geti();
        test.Nodes[0].Nodes.Add(rd);


        Match(TokenRec.TokenType.READ, currentToken);
        currentToken = tokenQueue.Dequeue();
        string id;
        id = currentToken.getTokenValue();
        geti();
        test.Nodes[0].Nodes.Add(id);
        //i = 1;
        Console.WriteLine(currentToken.Token_Type);

        Match(TokenRec.TokenType.IDENTIFIER, currentToken);
        if (tokenQueue.Count == 0)
        {
            Console.WriteLine("Error no semicolon");
            return currentToken;
        }
        else
        {
            currentToken = tokenQueue.Dequeue();
        }


        return currentToken;
    }


    private TokenRec writeStmt(TokenRec currentToken)
    {
        Match(TokenRec.TokenType.WRITE, currentToken);

        currentToken = tokenQueue.Dequeue();

        currentToken = exp(currentToken);

        return currentToken;
    }

    private TokenRec repeatStmt(TokenRec currentToken)
    {
        Match(TokenRec.TokenType.REPEAT, currentToken);

        currentToken = stmtSequence();

        Match(TokenRec.TokenType.UNTIL, currentToken);
        currentToken = tokenQueue.Dequeue();

        currentToken = exp(currentToken);

        return currentToken;
    }





    private TokenRec ifStmt(TokenRec currentToken)
    {

        Match(TokenRec.TokenType.IF, currentToken);
        currentToken = tokenQueue.Dequeue();

        currentToken = exp(currentToken);


        Match(TokenRec.TokenType.THEN, currentToken);
        currentToken = stmtSequence();


        if (currentToken.Token_Type == TokenRec.TokenType.ELSE)
        {
            Match(TokenRec.TokenType.ELSE, currentToken);
            currentToken = stmtSequence();
        }
        else if (currentToken.Token_Type == TokenRec.TokenType.END)
        {
            Match(TokenRec.TokenType.END, currentToken);
        }
        return currentToken;
    }


    public TokenRec assignStmt(TokenRec currentToken)
    {

        // create node and send valueto it string -> currentToken.getTokenValue
        Match(TokenRec.TokenType.IDENTIFIER, currentToken);
        string s = currentToken.getTokenValue();

        test.Nodes[0].Nodes.Add(s);

        TokenRec nextToken = tokenQueue.Dequeue();

        Match(TokenRec.TokenType.ASSIGNMENT, nextToken);

        nextToken = tokenQueue.Dequeue();



        currentToken = exp(nextToken);

        return currentToken;


    }

    public TokenRec exp(TokenRec currentToken)
    {
        currentToken = simpleExp(currentToken);


        if (tokenQueue.Count != 0 && (currentToken.Token_Type == TokenRec.TokenType.LESS_THAN || currentToken.Token_Type == TokenRec.TokenType.EQUAL))
        {

            Match(currentToken.Token_Type, currentToken);
             comp = currentToken.getTokenValue();
            if (comp != "=")
            {
                geti();
                test.Nodes[0].Nodes[i].Nodes.Add(comp);
                TokenRec nextToken = tokenQueue.Dequeue();
                //lw fe addop rag3a b3d comp
                if (addop == "+" || addop == "-" || addop == "*" || addop == "/")
                {
                    getj();

                    test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(addop);
                    getk();

                    test.Nodes[0].Nodes[i].Nodes[j].Nodes[k].Nodes.Add(term1);

                    test.Nodes[0].Nodes[i].Nodes[j].Nodes[k].Nodes.Add(term2);
                }
                else //lw mfe4 addop rag3a bs fe comp
                {
                    getj();
                    test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(term1);
                }

                currentToken = simpleExp(nextToken);
                //lw fe addop tanya 8er l addop al ola wfe comp 
                if (currentToken != null && comp != null)
                {
                    if (addop == "+" || addop == "-" || addop == "*" || addop == "/")
                    {
                        test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(addop);
                        getk();

                        test.Nodes[0].Nodes[i].Nodes[j].Nodes[k].Nodes.Add(term1);

                        test.Nodes[0].Nodes[i].Nodes[j].Nodes[k].Nodes.Add(term2);
                    }
                }
                //lw mfe4 addop rag3a wfe comp da l term l tany
                if (addop != "+" && addop != "-" && addop != "*" && addop != "/")
                {
                    term2 = nextToken.getTokenValue();
                    test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(term2);
                }

            }
        }
        return currentToken;


    }

    private TokenRec simpleExp(TokenRec currentToken)
    {
        
          term1 = currentToken.getTokenValue().ToString();
        

        currentToken = term(currentToken);
        //awl rkm fl exp term1


        while (tokenQueue.Count != 0 && currentToken.Token_Type == TokenRec.TokenType.ADDOP)
        {
            Console.WriteLine(currentToken.getTokenValue());
            Match(currentToken.Token_Type, currentToken);
              addop = currentToken.getTokenValue();
            
            TokenRec nextToken = tokenQueue.Dequeue();

             term2 = nextToken.getTokenValue();

            currentToken = term(nextToken);

               string comp2 = currentToken.getTokenValue();
            // lw fe addop wmfe4 comp 
            if (addop == "+") { 
                if (comp2 != "<" && comp2 != ">" && comp2 != "="  && comp2 != "" && addop != null)
                {
                    if (comp != "<" && comp != ">" && comp != "=" || comp == "" ) {
                        geti();
                        test.Nodes[0].Nodes[i].Nodes.Add(addop);
                        getj();
                        test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(term1);
                        test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(term2);
                    }
                }
            }
        }

        return currentToken;
    }


    private TokenRec term(TokenRec currentToken)
    {
        //string numalone;
        //numalone = currentToken.getTokenValue();
        currentToken = factor(currentToken);////////////
        string numaloneflag;
        numaloneflag = currentToken.getTokenValue();
        if (numaloneflag != "*" && numaloneflag == ";" && addop != "+" && comp != "<" && comp != ">")
        {
            geti();
            test.Nodes[0].Nodes[i].Nodes.Add(term1);
        }


        while (tokenQueue.Count != 0 && currentToken.Token_Type == TokenRec.TokenType.MULOP)
        {
            Console.WriteLine(currentToken.getTokenValue());
            string mulop = currentToken.getTokenValue();
            geti();
            test.Nodes[0].Nodes[i].Nodes.Add(mulop);
            getj();
            test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(term1);
            Match(currentToken.Token_Type, currentToken);
         
            TokenRec nextToken = tokenQueue.Dequeue();



            string mul2f = nextToken.getTokenValue();
            currentToken = factor(nextToken);
            test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(mul2f);
            //term2 for 2
        }
        return currentToken;

    }


    private TokenRec factor(TokenRec currentToken)
    {
        switch (currentToken.Token_Type)
        {
            case TokenRec.TokenType.LEFTBRACKET:
                Match(TokenRec.TokenType.LEFTBRACKET, currentToken);
                Console.WriteLine(currentToken.getTokenValue());
                TokenRec nextToken = tokenQueue.Dequeue();
                nextToken = exp(nextToken);



                Match(TokenRec.TokenType.RIGHTBRACKET, nextToken);
                Console.WriteLine(currentToken.getTokenValue());

                if (tokenQueue.Count != 0)
                    return tokenQueue.Dequeue();
                else
                    return nextToken;


            case TokenRec.TokenType.NUM:


                Match(TokenRec.TokenType.NUM, currentToken);

                Console.WriteLine(currentToken.getTokenValue());

                if (tokenQueue.Count != 0)
                    return tokenQueue.Dequeue();
                else
                    return currentToken;


            case TokenRec.TokenType.IDENTIFIER:
                Match(TokenRec.TokenType.IDENTIFIER, currentToken);

                if (tokenQueue.Count != 0)
                    return tokenQueue.Dequeue();
                else
                    return currentToken;


            default:
                throw new SystemException("Invalid token found.");
        }





    }



    void Match(TokenRec.TokenType expected, TokenRec currentToken)
    {

        if (currentToken.Token_Type == expected)
        {

        }

        else
            throw new SystemException("Invalid token found.");

    }








 
     public void geti()
    {
        i++;
      

    }
    public void getj()
    {
        j++;
       

    }
    public void getk()
    {
        k++;   

    }
    public void geth()
    {
        h++; 

    }
    TreeView test;
    public void gettree( ref TreeView treeview)
    {
        test = treeview;
    }
    
}
