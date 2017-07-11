# -*- coding: utf-8 -*-
"""
Created on Tue Jul 11 10:42:42 2017

@author: wujunchang
"""

import sys
import os
import subprocess
import argparse

def myHelp():
    #parser = argparse.ArgumentParser()
    #args = parser.parse_args()
    #print args
    print("python %s out_dir proto_files_dir" % sys.argv[0])

def argvParse():
    parser = argparse.ArgumentParser(prog="ProtoBuf 3 Compiler ")
    parser.add_argument("-o", "--outDir", type=str, required=True, help="Compile .proto file to the out directory.")
    parser.add_argument("-i", "--inDir", type = str, required=True, help="The .proto files locate directory.")
    parser.add_argument("-v", "--version", action="version", version="%(prog)s v1.0")

    args = parser.parse_args()
    return args

def doCmd(f, inDir, outDir, lang="csharp"):
    exe = 'protoc --proto_path=' + inDir + ' --' + lang + "_out=" + outDir + " " + f
    if os.name == 'nt':
        startupinfo = subprocess.STARTUPINFO()
        startupinfo.dwFlags |= subprocess.STARTF_USESHOWWINDOW
        startupinfo.wShowWindow = subprocess.SW_HIDE
    else:
        startupinfo = None
    
    subprocess.Popen(exe, startupinfo=startupinfo)
    
def main():
    args = argvParse()
    outDir = args.outDir
    inDir = args.inDir    

    for p in os.listdir(inDir):
        f = inDir + "/" + p
        if os.path.isfile(f) and p.endswith(".proto"):
            doCmd(f, inDir, outDir)
        
        
if __name__ == "__main__":
    main()