using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using WIN_SHORTCUTS_CL.Structure;

namespace WIN_SHORTCUTS_CL.Functions
{

    /// <summary>
    /// ## 읽기/쓰기 작업 후 클래스 자동 Dispose ##
    /// 이 클래스는 사용중일 때 DTO의 Stream에 사용중인 스트림을 저장한다
    /// 스트림은 사용되거나 사용중 여부를 확인하는데 이용되기 때문에 FileStream을 잡고 있는다.
    /// 그래서 사용 후 꼭 Dispose를 통해 DTO의 스트림을 제거해야 한다.
    /// </summary>
    internal class ScConfFileTool : IDisposable
    {
        #region Dispose 구현
        public void Dispose()
        {
            KeyboardDTO.CurrentUserKeyPairFile.Close();
            KeyboardDTO.CurrentUserKeyPairFile.Dispose();
            KeyboardDTO.CurrentUserKeyPairFile = null;

            GC.Collect();
        }

        #endregion

        //Init.
        internal ScConfFileTool(string _confFilePath)
        {
            // 파일이 사용중일 경우 행동 취소
            if(KeyboardDTO.CurrentUserKeyPairFile != null)
            {
                return;
            }

            // 파일을 생성하고 Open 해서 스트림을 점유한다
            ConnectOrCreateConfFileStream(_confFilePath);
        }

        /// <summary>
        /// // 파일을 생성하고 Open 해서 스트림을 점유한다
        /// </summary>
        /// <param name="_confFilePath">대상 파일의 경로</param>
        /// <returns>성공 시 1, 실패 시 ScDllException</returns>
        private int ConnectOrCreateConfFileStream(string _confFilePath)
        {
            try
            {
                // 파일이 존재하지 않는다면 해당 경로에 파일을 쓰고 Open한다.
                var _confFileStreamWriter = new StreamWriter(_confFilePath, true)
                {
                    AutoFlush = true
                };

                KeyboardDTO.CurrentUserKeyPairFile = _confFileStreamWriter.BaseStream;
            }
            catch (ScDllException se) { throw se; }

            return 1;
        }

        /// <summary>
        /// 현재 설정된 데이터를 파일스트림에 작성하여 저장한다
        /// (주의) 완료 후 파일 점유를 해제하고 클래스를 Dispose해서 정리한다.
        /// </summary>
        /// <param name="_shortCutsPairs">설정된 단축키 데이터 목록</param>
        /// <returns>성공 시 1, 실패 시 예외</returns>
        /// <exception cref="ScDllException"></exception>
        internal int WriteConfFileStream(ShortCutsPairList _shortCutsPairs)
        {
            string strLine = string.Empty;

            try
            {
                if(KeyboardDTO.CurrentUserKeyPairFile == null && KeyboardDTO.CurrentUserKeyPairFile.CanWrite == false)
                {
                    throw new ScDllException("저장된 파일을 이용할 수 없음");
                }

                //Stream을 새로 작성한다. (enc: UTF-8)
                using (var _confFileStreamWriter = new StreamWriter(KeyboardDTO.CurrentUserKeyPairFile, System.Text.Encoding.UTF8, 2048, false))
                {
                    try
                    {
                        //Pairs를 읽어 분리한다.
                        foreach(var _pair in _shortCutsPairs)
                        {
                            strLine = $"{_pair.Alias}:{(uint)_pair.ModifierKey}:{(uint)_pair.DataKey}:{(uint)_pair.Action}:{_pair.Target}";
                            _confFileStreamWriter.WriteLine(strLine);

                            strLine = string.Empty;
                        }
                    }
                    catch(ScDllException se)
                    {
                        throw se;
                    }
                    finally //Stream을 Close 후 Dispose한다.
                    {
                        _confFileStreamWriter.Close();
                        _confFileStreamWriter.Dispose();
                    }
                }
            }
            catch (ScDllException se)
            {
                throw se;
            }
            finally //Class를 Dispose한다. 
            {
                this.Dispose();
            }

            return 1;
        }

        /// <summary>
        /// 현재 스트림을 읽어 사용자 데이터를 추출하고 저장한다.
        /// (주의) 완료 후 파일 점유를 해제하고 클래스를 Dispose해서 정리한다.
        /// </summary>
        /// <returns>수집한 ShortCutsPairList를 반환한다</returns>
        /// <exception cref="ScDllException"></exception>
        internal ShortCutsPairList ReadConfFileStream()
        {
            if(KeyboardDTO.CurrentUserKeyPairFile == null || KeyboardDTO.CurrentUserKeyPairFile.CanRead == false)
            {
                throw new ScDllException("저장된 파일을 이용할 수 없음");
            }

            List<ShortCutsPair> _shortCutsPairList = null;
            ShortCutsPairList shortCutsPairs = null;

            string strLine = string.Empty;

            try
            {
                //Stream을 새로 연다. (enc: UTF-8)
                using(var _confFileStreamReader = new StreamReader(KeyboardDTO.CurrentUserKeyPairFile, System.Text.Encoding.UTF8))
                {
                    _shortCutsPairList = new List<ShortCutsPair>();
                    try
                    {
                        //Stream 을 라인 단위로 불러온다.
                        while ((strLine = _confFileStreamReader.ReadLine()) != null)
                        {
                            if (strLine.StartsWith("#") == true) continue; //주석처리가 되어 있는 경우에 Line을 이동한다
                            if (strLine.IndexOf(":") == -1) continue; //Split 문자(:)가 없는 경우에 Line을 이동한다

                            string[] confValParser = strLine.Split(':');
                            if (confValParser.Length != 4) continue; //Split 문자가 4개가 아닐 경우에 Line을 이동한다.

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
                    catch(ScDllException se)
                    {
                        throw se;
                    }
                    finally //Stream을 Close 후 Dispose한다.
                    {
                        _confFileStreamReader.Close();
                        _confFileStreamReader.Dispose();
                    }

                    shortCutsPairs = new ShortCutsPairList(_shortCutsPairList.ToArray());
                }
            }
            catch (ScDllException se)
            {
                throw se;
            }
            finally //Class를 Dispose한다. 
            { 
                this.Dispose(); 
            }

            return shortCutsPairs;
        }

    }
}
