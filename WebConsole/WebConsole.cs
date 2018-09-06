using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DWORD = System.UInt32;
using BOOL = System.Boolean;
using ControlKeyState = System.UInt32;

namespace BarnardTechnology
{
    public class WebConsole
    {
        WebSocketComms.CommsServer _wsServer;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(uint nStdHandle);

        public enum ControlKeyStates
        {
            RIGHT_ALT_PRESSED = 0x1,
            LEFT_ALT_PRESSED = 0x2,
            RIGHT_CTRL_PRESSED = 0x4,
            LEFT_CTRL_PRESSED = 0x8,
            SHIFT_PRESSED = 0x10,
            NUMLOCK_ON = 0x20,
            SCROLLLOCK_ON = 0x40,
            CAPSLOCK_ON = 0x80,
            ENHANCED_KEY = 0x100
        }

        /// <summary>
        /// Enumeration for virtual keys.
        /// </summary>
        public enum VirtualKeys
            : ushort
        {
            /// <summary></summary>
            LeftButton = 0x01,
            /// <summary></summary>
            RightButton = 0x02,
            /// <summary></summary>
            Cancel = 0x03,
            /// <summary></summary>
            MiddleButton = 0x04,
            /// <summary></summary>
            ExtraButton1 = 0x05,
            /// <summary></summary>
            ExtraButton2 = 0x06,
            /// <summary></summary>
            Back = 0x08,
            /// <summary></summary>
            Tab = 0x09,
            /// <summary></summary>
            Clear = 0x0C,
            /// <summary></summary>
            Return = 0x0D,
            /// <summary></summary>
            Shift = 0x10,
            /// <summary></summary>
            Control = 0x11,
            /// <summary></summary>
            Menu = 0x12,
            /// <summary></summary>
            Pause = 0x13,
            /// <summary></summary>
            CapsLock = 0x14,
            /// <summary></summary>
            Kana = 0x15,
            /// <summary></summary>
            Hangeul = 0x15,
            /// <summary></summary>
            Hangul = 0x15,
            /// <summary></summary>
            Junja = 0x17,
            /// <summary></summary>
            Final = 0x18,
            /// <summary></summary>
            Hanja = 0x19,
            /// <summary></summary>
            Kanji = 0x19,
            /// <summary></summary>
            Escape = 0x1B,
            /// <summary></summary>
            Convert = 0x1C,
            /// <summary></summary>
            NonConvert = 0x1D,
            /// <summary></summary>
            Accept = 0x1E,
            /// <summary></summary>
            ModeChange = 0x1F,
            /// <summary></summary>
            Space = 0x20,
            /// <summary></summary>
            Prior = 0x21,
            /// <summary></summary>
            Next = 0x22,
            /// <summary></summary>
            End = 0x23,
            /// <summary></summary>
            Home = 0x24,
            /// <summary></summary>
            Left = 0x25,
            /// <summary></summary>
            Up = 0x26,
            /// <summary></summary>
            Right = 0x27,
            /// <summary></summary>
            Down = 0x28,
            /// <summary></summary>
            Select = 0x29,
            /// <summary></summary>
            Print = 0x2A,
            /// <summary></summary>
            Execute = 0x2B,
            /// <summary></summary>
            Snapshot = 0x2C,
            /// <summary></summary>
            Insert = 0x2D,
            /// <summary></summary>
            Delete = 0x2E,
            /// <summary></summary>
            Help = 0x2F,
            /// <summary></summary>
            N0 = 0x30,
            /// <summary></summary>
            N1 = 0x31,
            /// <summary></summary>
            N2 = 0x32,
            /// <summary></summary>
            N3 = 0x33,
            /// <summary></summary>
            N4 = 0x34,
            /// <summary></summary>
            N5 = 0x35,
            /// <summary></summary>
            N6 = 0x36,
            /// <summary></summary>
            N7 = 0x37,
            /// <summary></summary>
            N8 = 0x38,
            /// <summary></summary>
            N9 = 0x39,
            /// <summary></summary>
            A = 0x41,
            /// <summary></summary>
            B = 0x42,
            /// <summary></summary>
            C = 0x43,
            /// <summary></summary>
            D = 0x44,
            /// <summary></summary>
            E = 0x45,
            /// <summary></summary>
            F = 0x46,
            /// <summary></summary>
            G = 0x47,
            /// <summary></summary>
            H = 0x48,
            /// <summary></summary>
            I = 0x49,
            /// <summary></summary>
            J = 0x4A,
            /// <summary></summary>
            K = 0x4B,
            /// <summary></summary>
            L = 0x4C,
            /// <summary></summary>
            M = 0x4D,
            /// <summary></summary>
            N = 0x4E,
            /// <summary></summary>
            O = 0x4F,
            /// <summary></summary>
            P = 0x50,
            /// <summary></summary>
            Q = 0x51,
            /// <summary></summary>
            R = 0x52,
            /// <summary></summary>
            S = 0x53,
            /// <summary></summary>
            T = 0x54,
            /// <summary></summary>
            U = 0x55,
            /// <summary></summary>
            V = 0x56,
            /// <summary></summary>
            W = 0x57,
            /// <summary></summary>
            X = 0x58,
            /// <summary></summary>
            Y = 0x59,
            /// <summary></summary>
            Z = 0x5A,
            /// <summary></summary>
            LeftWindows = 0x5B,
            /// <summary></summary>
            RightWindows = 0x5C,
            /// <summary></summary>
            Application = 0x5D,
            /// <summary></summary>
            Sleep = 0x5F,
            /// <summary></summary>
            Numpad0 = 0x60,
            /// <summary></summary>
            Numpad1 = 0x61,
            /// <summary></summary>
            Numpad2 = 0x62,
            /// <summary></summary>
            Numpad3 = 0x63,
            /// <summary></summary>
            Numpad4 = 0x64,
            /// <summary></summary>
            Numpad5 = 0x65,
            /// <summary></summary>
            Numpad6 = 0x66,
            /// <summary></summary>
            Numpad7 = 0x67,
            /// <summary></summary>
            Numpad8 = 0x68,
            /// <summary></summary>
            Numpad9 = 0x69,
            /// <summary></summary>
            Multiply = 0x6A,
            /// <summary></summary>
            Add = 0x6B,
            /// <summary></summary>
            Separator = 0x6C,
            /// <summary></summary>
            Subtract = 0x6D,
            /// <summary></summary>
            Decimal = 0x6E,
            /// <summary></summary>
            Divide = 0x6F,
            /// <summary></summary>
            F1 = 0x70,
            /// <summary></summary>
            F2 = 0x71,
            /// <summary></summary>
            F3 = 0x72,
            /// <summary></summary>
            F4 = 0x73,
            /// <summary></summary>
            F5 = 0x74,
            /// <summary></summary>
            F6 = 0x75,
            /// <summary></summary>
            F7 = 0x76,
            /// <summary></summary>
            F8 = 0x77,
            /// <summary></summary>
            F9 = 0x78,
            /// <summary></summary>
            F10 = 0x79,
            /// <summary></summary>
            F11 = 0x7A,
            /// <summary></summary>
            F12 = 0x7B,
            /// <summary></summary>
            F13 = 0x7C,
            /// <summary></summary>
            F14 = 0x7D,
            /// <summary></summary>
            F15 = 0x7E,
            /// <summary></summary>
            F16 = 0x7F,
            /// <summary></summary>
            F17 = 0x80,
            /// <summary></summary>
            F18 = 0x81,
            /// <summary></summary>
            F19 = 0x82,
            /// <summary></summary>
            F20 = 0x83,
            /// <summary></summary>
            F21 = 0x84,
            /// <summary></summary>
            F22 = 0x85,
            /// <summary></summary>
            F23 = 0x86,
            /// <summary></summary>
            F24 = 0x87,
            /// <summary></summary>
            NumLock = 0x90,
            /// <summary></summary>
            ScrollLock = 0x91,
            /// <summary></summary>
            NEC_Equal = 0x92,
            /// <summary></summary>
            Fujitsu_Jisho = 0x92,
            /// <summary></summary>
            Fujitsu_Masshou = 0x93,
            /// <summary></summary>
            Fujitsu_Touroku = 0x94,
            /// <summary></summary>
            Fujitsu_Loya = 0x95,
            /// <summary></summary>
            Fujitsu_Roya = 0x96,
            /// <summary></summary>
            LeftShift = 0xA0,
            /// <summary></summary>
            RightShift = 0xA1,
            /// <summary></summary>
            LeftControl = 0xA2,
            /// <summary></summary>
            RightControl = 0xA3,
            /// <summary></summary>
            LeftMenu = 0xA4,
            /// <summary></summary>
            RightMenu = 0xA5,
            /// <summary></summary>
            BrowserBack = 0xA6,
            /// <summary></summary>
            BrowserForward = 0xA7,
            /// <summary></summary>
            BrowserRefresh = 0xA8,
            /// <summary></summary>
            BrowserStop = 0xA9,
            /// <summary></summary>
            BrowserSearch = 0xAA,
            /// <summary></summary>
            BrowserFavorites = 0xAB,
            /// <summary></summary>
            BrowserHome = 0xAC,
            /// <summary></summary>
            VolumeMute = 0xAD,
            /// <summary></summary>
            VolumeDown = 0xAE,
            /// <summary></summary>
            VolumeUp = 0xAF,
            /// <summary></summary>
            MediaNextTrack = 0xB0,
            /// <summary></summary>
            MediaPrevTrack = 0xB1,
            /// <summary></summary>
            MediaStop = 0xB2,
            /// <summary></summary>
            MediaPlayPause = 0xB3,
            /// <summary></summary>
            LaunchMail = 0xB4,
            /// <summary></summary>
            LaunchMediaSelect = 0xB5,
            /// <summary></summary>
            LaunchApplication1 = 0xB6,
            /// <summary></summary>
            LaunchApplication2 = 0xB7,
            /// <summary></summary>
            OEM1 = 0xBA,
            /// <summary></summary>
            OEMPlus = 0xBB,
            /// <summary></summary>
            OEMComma = 0xBC,
            /// <summary></summary>
            OEMMinus = 0xBD,
            /// <summary></summary>
            OEMPeriod = 0xBE,
            /// <summary></summary>
            OEM2 = 0xBF,
            /// <summary></summary>
            OEM3 = 0xC0,
            /// <summary></summary>
            OEM4 = 0xDB,
            /// <summary></summary>
            OEM5 = 0xDC,
            /// <summary></summary>
            OEM6 = 0xDD,
            /// <summary></summary>
            OEM7 = 0xDE,
            /// <summary></summary>
            OEM8 = 0xDF,
            /// <summary></summary>
            OEMAX = 0xE1,
            /// <summary></summary>
            OEM102 = 0xE2,
            /// <summary></summary>
            ICOHelp = 0xE3,
            /// <summary></summary>
            ICO00 = 0xE4,
            /// <summary></summary>
            ProcessKey = 0xE5,
            /// <summary></summary>
            ICOClear = 0xE6,
            /// <summary></summary>
            Packet = 0xE7,
            /// <summary></summary>
            OEMReset = 0xE9,
            /// <summary></summary>
            OEMJump = 0xEA,
            /// <summary></summary>
            OEMPA1 = 0xEB,
            /// <summary></summary>
            OEMPA2 = 0xEC,
            /// <summary></summary>
            OEMPA3 = 0xED,
            /// <summary></summary>
            OEMWSCtrl = 0xEE,
            /// <summary></summary>
            OEMCUSel = 0xEF,
            /// <summary></summary>
            OEMATTN = 0xF0,
            /// <summary></summary>
            OEMFinish = 0xF1,
            /// <summary></summary>
            OEMCopy = 0xF2,
            /// <summary></summary>
            OEMAuto = 0xF3,
            /// <summary></summary>
            OEMENLW = 0xF4,
            /// <summary></summary>
            OEMBackTab = 0xF5,
            /// <summary></summary>
            ATTN = 0xF6,
            /// <summary></summary>
            CRSel = 0xF7,
            /// <summary></summary>
            EXSel = 0xF8,
            /// <summary></summary>
            EREOF = 0xF9,
            /// <summary></summary>
            Play = 0xFA,
            /// <summary></summary>
            Zoom = 0xFB,
            /// <summary></summary>
            Noname = 0xFC,
            /// <summary></summary>
            PA1 = 0xFD,
            /// <summary></summary>
            OEMClear = 0xFE
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
            public bool bKeyDown;
            [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
            public ushort wRepeatCount;
            [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
            public VirtualKeys wVirtualKeyCode;
            [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
            public ushort wVirtualScanCode;
            [FieldOffset(10)]
            public char UnicodeChar;
            [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
            public ControlKeyState dwControlKeyState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSE_EVENT_RECORD
        {
            [FieldOffset(0)]
            public COORD dwMousePosition;
            [FieldOffset(4)]
            public uint dwButtonState;
            [FieldOffset(8)]
            public uint dwControlKeyState;
            [FieldOffset(12)]
            public uint dwEventFlags;
        }

        public struct WINDOW_BUFFER_SIZE_RECORD
        {
            public COORD dwSize;

            public WINDOW_BUFFER_SIZE_RECORD(short x, short y)
            {
                this.dwSize = new COORD(x, y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MENU_EVENT_RECORD
        {
            public uint dwCommandId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FOCUS_EVENT_RECORD
        {
            public uint bSetFocus;
        }


        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public ushort EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
            [FieldOffset(4)]
            public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
            [FieldOffset(4)]
            public MENU_EVENT_RECORD MenuEvent;
            [FieldOffset(4)]
            public FOCUS_EVENT_RECORD FocusEvent;
        };

        /* Writes data directly to the console input buffer. */
        [DllImport("kernel32.dll", EntryPoint = "WriteConsoleInputW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern BOOL WriteConsoleInput(
            IntPtr hConsoleInput,
            [MarshalAs(UnmanagedType.LPArray), In] INPUT_RECORD[] lpBuffer,
            DWORD nLength,
            out DWORD lpNumberOfEventsWritten);

        public class ConsoleKeyPress
        {
            public char Value;
            public ConsoleColor ForegroundColor;
            public ConsoleColor BackgroundColor;
        }

        public WebConsole()
        {

            string[] prefix =
            {
                "http://localhost:8088/"
            };

            WebConsoleContent.Content c = new WebConsoleContent.Content();

            EmbeddedWebServer.ContentServer _server = new EmbeddedWebServer.ContentServer(prefix, c.GetType(), (request) =>
            {
                WebServer.Server.ResponseInformation resp = null;

                if(request.Url.AbsolutePath == "/console.txt")
                {
                    resp = new WebServer.Server.ResponseInformation();
                    resp.Content = System.Text.Encoding.UTF8.GetBytes(tOut.GetStringBuilder().ToString());
                    resp.ContentType = "text/plain";
                }

                return resp;
            });

            _wsServer = new WebSocketComms.CommsServer("webconsole", 888, false, true);
            _wsServer.AddRoute("webconsole", this, null);
            MirrorConsole();
        }

        public List<ConsoleKeyPress> GetBuffer()
        {
            return dWriter.GetBuffer();
        }

        public void SendKeyPress(int charCode)
        {
            //foreach(char key in keys)
            //{
            char key = (char)charCode;
            char keyU = key.ToString().ToUpper().ToCharArray()[0];
            uint STD_INPUT_HANDLE = 0xfffffff6;
            IntPtr hConsoleInput = GetStdHandle(STD_INPUT_HANDLE);
            INPUT_RECORD[] iR = new INPUT_RECORD[]
            {
            new INPUT_RECORD()
            {
                KeyEvent = new KEY_EVENT_RECORD()
                {
                    bKeyDown = true,
                    UnicodeChar = key,
                    wVirtualKeyCode = (VirtualKeys)keyU
                },
                EventType = 0x1
            }
            };

            uint eventsWritten;
            WriteConsoleInput(hConsoleInput, iR, 1, out eventsWritten);
            //}
        }

        class DoubleWriter : TextWriter
        {
            TextWriter one;
            TextWriter two;
            WebSocketComms.CommsServer _comms;
            List<ConsoleKeyPress> _consoleBuffer;

            public DoubleWriter(TextWriter one, TextWriter two, WebSocketComms.CommsServer commsOutput)
            {
                _consoleBuffer = new List<ConsoleKeyPress>();
                this.one = one;
                this.two = two;
                _comms = commsOutput;
            }

            public override Encoding Encoding
            {
                get { return one.Encoding; }
            }

            public override void Flush()
            {
                one.Flush();
                two.Flush();
            }

            public override void Write(char value)
            {
                one.Write(value);
                two.Write(value);

                _comms.SendMessage(new WebSocketComms.TCPCommand("WriteChar", new List<object>() { value, Console.ForegroundColor.ToString(), Console.BackgroundColor.ToString() }, null));
                _consoleBuffer.Add(new ConsoleKeyPress() { Value = value, ForegroundColor = Console.ForegroundColor, BackgroundColor = Console.BackgroundColor });
            }

            public List<ConsoleKeyPress> GetBuffer()
            {
                return _consoleBuffer;
            }
        }
        DoubleWriter dWriter;
        StringWriter tOut;

        /// <summary>
        /// The aim of the ConsoleReader is to echo any keypresses from a Console.ReadLine() out to both the
        /// standard console and to the WebConsole. Without redirecting the Console.In to a the ConsoleReader,
        /// the keypresses would not be passed on to the WebConsole.
        /// Also, the ConsoleReader can push text back to the Console from WebConsole session.
        /// </summary>
        class ConsoleReader : TextReader
        {
            public override int Peek()
            {
                return base.Peek();
            }

            public override int Read()
            {
                return base.Read();
            }

            public override string ReadLine()
            {
                string inString = "";
                ConsoleKeyInfo nextKey = Console.ReadKey(true);
                while(nextKey.Key != ConsoleKey.Enter)
                {
                    if(nextKey.Key == ConsoleKey.Backspace)
                    {
                        if (inString.Length > 0)
                        {
                            inString = inString.Substring(0, inString.Length - 1);
                            Console.Write("\b \b");
                        }
                    }
                    else if(!char.IsControl(nextKey.KeyChar))
                    {
                        inString += nextKey.KeyChar;
                        Console.Write(nextKey.KeyChar);
                    }
                    //Console.Write(nextKey.KeyChar);
                    nextKey = Console.ReadKey(true);
                }
                Console.WriteLine();
                return inString;
                //return base.ReadLine();
            }
        }

        void MirrorConsole()
        {
            tOut = new StringWriter();
            dWriter = new DoubleWriter(tOut, Console.Out, _wsServer);
            Console.SetOut(dWriter);

            ConsoleReader tReader = new ConsoleReader();
            Console.SetIn(tReader);
        }
    }
}
