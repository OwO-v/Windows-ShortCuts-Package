using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using WIN_SHORTCUTS_CL.Structure;

namespace WIN_SHORTCUTS_CL.Functions
{

    /// <summary>
    /// ## 읽기/쓰기 작업 후 클래스 자동 Dispose ##
    /// 이 클래스는 사용될때마다 DTO의 Stream에 사용중인 스트림을 저장한다
    /// 스트림은 사용되거나 사용중 여부를 확인하는데 이용되기 때문에 FileStream을 잡고 있는다.
    /// 그래서 사용 후 꼭 Dispose를 통해 DTO의 스트림을 제거해야 한다.
    /// </summary>
    internal class ScConfFileTool : IDisposable
    {
        #region Dispose 구현
        public void Dispose()
        {
            ConfFileStreamTrace.Close();
            ConfFileStreamTrace.Dispose();
            ConfFileStreamTrace = null;

            KeyboardDTO.CurrentUserKeyPairFile.Dispose();
            KeyboardDTO.CurrentUserKeyPairFile = null;

            GC.Collect();
        }

        #endregion

        private string ConfFilePath = string.Empty;
        private Stream ConfFileStreamTrace = null;


        internal ScConfFileTool(string _confFilePath)
        {
            if(KeyboardDTO.CurrentUserKeyPairFile != null)
            {
                return;
            }

            ConnectOrCreateConfFileStream(_confFilePath);
        }
        

        private int ConnectOrCreateConfFileStream(string _confFilePath)
        {
            try
            {
                var _confFileStreamWriter = new StreamWriter(_confFilePath, true)
                {
                    AutoFlush = true
                };

                this.ConfFileStreamTrace = _confFileStreamWriter.BaseStream;
                KeyboardDTO.CurrentUserKeyPairFile = ConfFileStreamTrace;
            }
            catch (IOException ie) { throw ie; }
            catch (Exception ex) { throw ex; }

            ConfFilePath = _confFilePath;
            return 1;
        }

        internal int WriteConfFileStream(ShortCutsPairList _shortCutsPairs)
        {
            string strLine = string.Empty;

            try
            {
                if(KeyboardDTO.CurrentUserKeyPairFile == null && this.ConfFileStreamTrace.CanWrite == false)
                {
                    throw new ScDllException("저장된 파일을 이용할 수 없음");
                }


                using (var _confFileStreamWriter = new StreamWriter(ConfFileStreamTrace, System.Text.Encoding.UTF8))
                {
                    try
                    {
                        foreach(var _pair in _shortCutsPairs)
                        {
                            strLine = $"{_pair.Alias}:{(uint)_pair.ModifierKey}:{(uint)_pair.DataKey}:{(uint)_pair.Action}:{_pair.Target}";
                            _confFileStreamWriter.WriteLine(strLine);

                            strLine = string.Empty;
                        }
                    }
                    catch(IOException ie)
                    {
                        throw ie;
                    }
                    finally
                    {
                        _confFileStreamWriter.Close();
                        _confFileStreamWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }

            return 1;
        }

        internal ShortCutsPairList ReadConfFileStream()
        {
            if(KeyboardDTO.CurrentUserKeyPairFile == null || this.ConfFileStreamTrace.CanRead == false)
            {
                throw new IOException("저장된 파일을 이용할 수 없음");
            }

            List<ShortCutsPair> _shortCutsPairList = null;
            ShortCutsPairList shortCutsPairs = null;

            string strLine = string.Empty;

            try
            {
                using(var _confFileStreamReader = new StreamReader(ConfFileStreamTrace, System.Text.Encoding.UTF8))
                {
                    try
                    {
                        while ((strLine = _confFileStreamReader.ReadLine()) != null)
                        {
                            if (strLine.StartsWith("#") == true) continue;
                            if (strLine.IndexOf(":") == -1) continue;

                            string[] confValParser = strLine.Split(':');
                            if (confValParser.Length != 5) continue;

                            _shortCutsPairList.Add(
                                new ShortCutsPair(
                                    confValParser[0],
                                    (ModiKey)uint.Parse(confValParser[1]),
                                    (Key)uint.Parse(confValParser[2]),
                                    (ScAction)uint.Parse(confValParser[3]),
                                    confValParser[4]
                                )
                            );
                        }
                    }
                    catch(IOException ie)
                    {
                        throw ie;
                    }
                    finally
                    {
                        _confFileStreamReader.Close();
                        _confFileStreamReader.Dispose();
                    }
                }

                shortCutsPairs = new ShortCutsPairList(_shortCutsPairList.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            { 
                this.Dispose(); 
            }

            return shortCutsPairs;
        }

    }
}
