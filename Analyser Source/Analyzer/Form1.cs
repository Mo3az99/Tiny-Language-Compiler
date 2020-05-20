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

            Token scanner = new Token();
            scanner.getToken(str);
            richTextBox2.Text = scanner.slicer(scanner.GetText());

            parser = new Parser(scanner.getTokenQueue());
            parser.gettree(ref treeView1);

            parser.parse();
            


        }
        public void button3_Click(object sender, EventArgs e)
        {
            this.treeView1.BringToFront();

        
        }


   
    }
}
class Parser //:dParser.Form1
{
    string addop = "";
    string term1 = "";
    string term2 = "";
       
    Queue<TokenRec> tokenQueue = new Queue<TokenRec>();

    public Parser(Queue<TokenRec> tokenQueue)
    {
        this.tokenQueue = tokenQueue;
    }
    public void parse()
    {


       string s = "Program 0";
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
            TokenRec currentToken = tokenQueue.Dequeue();
        Console.WriteLine(currentToken.Token_Type);
        currentToken = statement(currentToken);

        while (tokenQueue.Count != 0 && currentToken.Token_Type == TokenRec.TokenType.SEMICOLON)
        {
            Match(TokenRec.TokenType.SEMICOLON, currentToken);
            

            currentToken = tokenQueue.Dequeue();


        }
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
                throw new SystemException("Invalid statement found.");

        }


    }

    private TokenRec readStmt(TokenRec currentToken)
    {

        Match(TokenRec.TokenType.READ, currentToken);
        currentToken = tokenQueue.Dequeue();
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
            string comp = currentToken.getTokenValue();
            geti();
            test.Nodes[0].Nodes[i].Nodes.Add(comp);
            TokenRec nextToken = tokenQueue.Dequeue();
            getj();
            test.Nodes[0].Nodes[i].Nodes[j].Nodes.Add(addop);
            getk();

            test.Nodes[0].Nodes[i].Nodes[j].Nodes[k].Nodes.Add(term1);

            test.Nodes[0].Nodes[i].Nodes[j].Nodes[k].Nodes.Add(term2);

            currentToken = simpleExp(nextToken);


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
   
        }

        return currentToken;
    }


    private TokenRec term(TokenRec currentToken)
    {
        currentToken = factor(currentToken);



        while (tokenQueue.Count != 0 && currentToken.Token_Type == TokenRec.TokenType.MULOP)
        {
            Console.WriteLine(currentToken.getTokenValue());
           
            Match(currentToken.Token_Type, currentToken);
         
            TokenRec nextToken = tokenQueue.Dequeue();



            currentToken = factor(nextToken);

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








     int  i = -1,j = -1,k = -1,h = -1;
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
