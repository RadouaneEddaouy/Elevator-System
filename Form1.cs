using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Net;
using System.Net.Mail;


namespace TpCalculette
{
    public partial class frmCalculette : Form
    {
        private readonly Elevator elevator = new Elevator();
        private static SoundPlayer elevatorSoundPlayer;

        public frmCalculette()
        {
            InitializeComponent();
            Elevator.SetPictureBox(elevatorPictureBox);
            Elevator.SetDoors(leftimage3, rightimage3, leftimage2, rightimage2, leftimage1, rightimage1);

            elevatorPictureBox.SendToBack();

              elevatorSoundPlayer = new SoundPlayer("E:/C#/AFTER TEST/Elevator Project/elevator.wav");
            Elevator.Setaudio(elevatorSoundPlayer);
          }

        private void frmCalculette_Load(object sender, EventArgs e)
        {

        }
        private void picCallTop_Click(object sender, EventArgs e)
        {
            elevator.AddToQueue(1);
        }

        private void picCallMiddle_Click(object sender, EventArgs e)
        {
            elevator.AddToQueue(2);
        }

        private void picCallBottom_Click(object sender, EventArgs e)
        {
            elevator.AddToQueue(3);
        }
        private void one_Click(object sender, EventArgs e)
        {
            elevator.AddToQueue(1);
        }

        private void two_Click(object sender, EventArgs e)
        {
            elevator.AddToQueue(2);
        }

        private void three_Click(object sender, EventArgs e)
        {
            elevator.AddToQueue(3);
        }

       
        private void leftimage1_Click(object sender, EventArgs e)
        {

        }
        private void openimage_Click(object sender, EventArgs e)
        { 
            elevator.OpenFloorDoors();
        }
        private void rightimage1_Click(object sender, EventArgs e)
        {

        }
        private void closeimage_Click(object sender, EventArgs e)
        {
            elevator.CloseFloorDoors();
        }
        private void btn_urgent_Click(object sender, EventArgs e)
        {
            /*
             *     ===    i try here to send msg by email === 
             try
             {
                 string senderEmail = "urgcsharptest@gmail.com";
                 string senderPassword = "********";

                 string recipientEmail = "redone2019ma@gmail.com";

                 string urgency = "Urgent!";
                 string subject = $"Attention: {urgency} Notification";
                 string body = $"There is an {urgency} notification. Please take immediate action.";

                 MailMessage mail = new MailMessage();
                 mail.From = new MailAddress(senderEmail);
                 mail.To.Add(recipientEmail);
                 mail.Subject = subject;
                 mail.Body = body;

                 SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                 smtp.Port = 587;
                 smtp.UseDefaultCredentials = false;
                 smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                 smtp.EnableSsl = true;

                 smtp.Send(mail);

                 MessageBox.Show($"Email sent successfully: {subject}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             catch (Exception ex)
             {
                 MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
            */
            MessageBox.Show(" Don't worry, a Message was sent to the team , they are coming");
        }

        private class Elevator
        {
            private static PictureBox elevatorPictureBox;
            private static readonly int Speed = 5;
            private static readonly int YUp = 32;
            private static readonly int YDown = 430;
            private static readonly int YMiddle = 230;

            private static int currentFloor = 1;
            private static Queue<int> floorQueue = new Queue<int>();
            private static bool isMoving = false;
            private bool doorsOpen = false;

            private static PictureBox leftDoorPictureBox;
            private static PictureBox rightDoorPictureBox;

            private static PictureBox leftDoorPictureBox2;
            private static PictureBox rightDoorPictureBox2;

            private static PictureBox leftDoorPictureBox3;
            private static PictureBox rightDoorPictureBox3;
            private int doorAnimationStep = 5;
            private Timer doorAnimationTimer;

            private static SoundPlayer aide;


            private enum DoorState
            {
                Closed,
                Opening,
                Opened,
                Closing
            }


            public static void Setaudio(SoundPlayer myAudio)
            {
                aide = myAudio;
            }

          


            private DoorState currentDoorState = DoorState.Closed;
            public static void SetPictureBox(PictureBox pictureBox)
            {
                elevatorPictureBox = pictureBox;
            }
            public static void SetDoors(PictureBox leftDoor, PictureBox rightDoor, PictureBox leftDoor2, PictureBox rightDoor2, PictureBox leftDoor3, PictureBox rightDoor3)
            {
                leftDoorPictureBox = leftDoor;
                rightDoorPictureBox = rightDoor;

                leftDoorPictureBox2 = leftDoor2;
                rightDoorPictureBox2 = rightDoor2;

                leftDoorPictureBox3 = leftDoor3;
                rightDoorPictureBox3 = rightDoor3;

            }

          

            public void AddToQueue(int targetFloor)
            {
                floorQueue.Enqueue(targetFloor);
                MoveToNextFloor();
            }


            private void MoveToNextFloor()
            {
                if (!isMoving && floorQueue.Count > 0)
                {
                    int targetFloor = floorQueue.Dequeue();
                    if (doorsOpening == true)
                    {
                        

                        Task.Delay(500).ContinueWith(_ =>
                        {
                            Console.WriteLine(" we will close the door");
                            CloseFloorDoors();
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                        Task.Delay(1500).ContinueWith(_ =>
                        {
                           
                            MoveToFloor(targetFloor);
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                        MoveToFloor(targetFloor);
                   
                }
            }


            private void MoveToFloor(int targetFloor)
            {
                Timer moveTimer = new Timer();
                moveTimer.Interval = 20;

                moveTimer.Tick += (sender, e) =>
                {

                    if (targetFloor == 1)
                    {
                        if (floorQueue.Contains(2))
                        {
                            MoveElevatorTofloor2(moveTimer);
                            Task.Delay(3000).ContinueWith(_ =>
                            {

                                floorQueue.Enqueue(1);
                                MoveToNextFloor();
                                isMoving = false;
                                floorQueue = new Queue<int>(floorQueue.Where(floor => floor != 2).ToList());
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                            return;
                        }
                        else
                        {
                            MoveElevatorTofloor1(moveTimer);
                        }
                    }
                    else if (targetFloor == 2)
                    {
                        MoveElevatorTofloor2(moveTimer);
                    }
                    else if (targetFloor == 3)
                    {
                        
                        if (floorQueue.Contains(2))
                        {
                            MoveElevatorTofloor2(moveTimer);
                           
                            Task.Delay(5000).ContinueWith(_ =>
                            {
                                floorQueue.Enqueue(3);
                                MoveToNextFloor();
                                isMoving = false;
                                floorQueue = new Queue<int>(floorQueue.Where(floor => floor != 2).ToList());
                               
                               
                                Console.WriteLine("Calling OpenDoors for floor 3");
                               
                             
                                currentDoorState = DoorState.Opening;
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                            return;
                        }
                        else
                        {
                            MoveElevatorTofloor3(moveTimer);
                        }
                    }

                    if (currentFloor == targetFloor)
                    {
                        moveTimer.Stop();
                        isMoving = false;
                        Console.WriteLine("Calling OpenDoors after reaching the target floor");
                        OpenFloorDoors();
                        MoveToNextFloor();
                    }
                };

                moveTimer.Start();
                isMoving = true;
            }
            private void MoveElevatorTofloor1(Timer moveTimer)
            {
                if (currentFloor == 2 || currentFloor == 3)
                {
                    elevatorPictureBox.Top = Speed + elevatorPictureBox.Top;
                    if (elevatorPictureBox.Location.Y >= YDown)
                    {
                        elevatorPictureBox.Top = YDown;
                        moveTimer.Stop();
                        currentFloor = 1;
                        isMoving = false;
                        MoveToNextFloor();
                        aide.Play();
                    }
                    Console.WriteLine($"Top: {elevatorPictureBox.Top}, current floor: {currentFloor}");
                }
            }


            private void MoveElevatorTofloor2(Timer moveTimer)
            {
                if (currentFloor == 3)
                {
                    elevatorPictureBox.Top = Speed + elevatorPictureBox.Top;
                    if (elevatorPictureBox.Location.Y >= YMiddle)
                    {
                        elevatorPictureBox.Top = YMiddle;
                        moveTimer.Stop();
                        currentFloor = 2;
                        isMoving = false;
                        MoveToNextFloor();
                        aide.Play();
                    }
                    Console.WriteLine($"Top: {elevatorPictureBox.Top}, current floor: {currentFloor}");
                }
                else if (currentFloor == 1)
                {
                    elevatorPictureBox.Top -= Speed;
                    if (elevatorPictureBox.Top <= YMiddle)
                    {
                        elevatorPictureBox.Top = YMiddle;
                        moveTimer.Stop();
                        currentFloor = 2;
                        isMoving = false;
                        MoveToNextFloor();
                        aide.Play();
                    }
                    Console.WriteLine($"Top: {elevatorPictureBox.Top}, current floor: {currentFloor}");
                }
                else if (currentFloor == 2 && floorQueue.Contains(3))
                {
                    moveTimer.Stop();
                    isMoving = false;
                    MoveToNextFloor();

                    Task.Delay(3000).Wait();

                    MoveToFloor(3);
                    aide.Play();
                }
            }

            private void MoveElevatorTofloor3(Timer moveTimer)
            {
                if (currentFloor == 1)
                {
                    elevatorPictureBox.Top -= Speed;
                    if (elevatorPictureBox.Top <= YUp)
                    {
                        elevatorPictureBox.Top = YUp;
                        moveTimer.Stop();
                        currentFloor = 3;
                        isMoving = false;
                        MoveToNextFloor();
                        aide.Play();
                    }
                    
                    Console.WriteLine($"Top: {elevatorPictureBox.Top}, current floor: {currentFloor}");
                  
                }
               
                else if (currentFloor == 2)
                {
                    elevatorPictureBox.Top -= Speed;
                    if (elevatorPictureBox.Top <= YUp)
                    {
                        Console.WriteLine("//////////////");
                        elevatorPictureBox.Top = YUp;
                        moveTimer.Stop();
                        currentFloor = 3;
                        isMoving = false;
                        MoveToNextFloor();
                        aide.Play();
                    }

                    Console.WriteLine($"Top: {elevatorPictureBox.Top}, current floor: {currentFloor}");
                }
            }
            private bool doorsOpening = false;
            internal void OpenDoors(PictureBox leftDoor, PictureBox rightDoor)
            {
                if (leftDoor != null && rightDoor != null)
                {
                    int leftDoorSpeed = 1;
                    int rightDoorSpeed = 1;

                    doorsOpening = false; // Set the boolean variable to true

                    doorAnimationTimer = new Timer();
                    doorAnimationTimer.Interval = 20;

                    doorAnimationTimer.Tick += (sender, e) =>
                    {
                        leftDoor.Left -= leftDoorSpeed;
                        rightDoor.Left += rightDoorSpeed;

                        Console.WriteLine($"Left: {leftDoor.Left}, Right: {rightDoor.Left}");

                        if (leftDoor.Left <= 55 && rightDoor.Left >= 250)
                        {
                            doorAnimationTimer.Stop();
                            currentDoorState = DoorState.Opening;
                            doorsOpening = true; // Reset the boolean variable when doors are fully opened
                            Console.WriteLine("Doors fully opened");
                           

                        }
                    };

                    doorAnimationTimer.Start();
                }
            }


            internal void OpenFloorDoors()
            {
                if (currentFloor == 1 && doorsOpening == false)
                {
                    OpenDoors(leftDoorPictureBox, rightDoorPictureBox);
                }
                else if (currentFloor == 2 && doorsOpening == false)
                {
                    OpenDoors(leftDoorPictureBox2, rightDoorPictureBox2);
                }
                else if (currentFloor == 3 && doorsOpening == false)
                {
                    OpenDoors(leftDoorPictureBox3, rightDoorPictureBox3);
                }
            }

            internal void CloseFloorDoors()
            {
                if (currentFloor == 1 && doorsOpening == true)
                {
                    CloseDoors(leftDoorPictureBox, rightDoorPictureBox);
                }
                else if (currentFloor == 2 && doorsOpening == true)
                {
                    CloseDoors(leftDoorPictureBox2, rightDoorPictureBox2);
                }
                else if (currentFloor == 3 && doorsOpening == true)
                {
                    CloseDoors(leftDoorPictureBox3, rightDoorPictureBox3);
                }
            }



            internal void CloseDoors(PictureBox leftDoor, PictureBox rightDoor)
            {
                if (leftDoor != null && rightDoor != null)
                {
                    int leftDoorSpeed = 1;
                    int rightDoorSpeed = 1;

                    doorAnimationTimer = new Timer();
                    doorAnimationTimer.Interval = 20;

                    doorAnimationTimer.Tick += (sender, e) =>
                    {
                        leftDoor.Left += leftDoorSpeed;
                        rightDoor.Left -= rightDoorSpeed;

                        Console.WriteLine($"Left: {leftDoor.Left}, Right: {rightDoor.Left}");

                        if (leftDoor.Left >= 87 && rightDoor.Left <= 192)
                        {
                             doorAnimationTimer.Stop();
                            Console.WriteLine("Condition met");
                           
                            currentDoorState = DoorState.Closed;
                            doorsOpening = false;
                            Console.WriteLine(currentDoorState);
                        }
                    };

                    doorAnimationTimer.Start();
                }
            }
             


        }

       
    }
}
