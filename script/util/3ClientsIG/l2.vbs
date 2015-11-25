' http://l2divine.com/forum/topic_view.jsp?c=Nw==&f=NxBPKWQ=#p46108
' 3 clients IG
' by noetonebeda on Wed Sep 04, 2013 02:38 AM
'
' run 1, 2 or 3 client
' run l2.vbs script (now you can start more than 3 client's)
' dont forget use script after run every 3 clients

Set objShell = CreateObject("WScript.Shell")
Set objExec = objShell.Exec("handle.exe -a -p l2.bin")

pid = ""
count = 0

Do
line = objExec.StdOut.ReadLine()
pos = InStr(line, "pid: ")
if pos <> 0 then
	pos = pos + 5
	p2 = InStr(pos, line, " ")
	if p2 <> 0 then
		pid = Mid(line, pos, p2 - pos)
		rem WScript.StdOut.WriteLine("Found pid: " & pid)
	end if
else
	pos = InStr(line, "Lineage2IsFreeRunning")
	if pos <> 0 then
		pos = InStr(line, ":")
		if pos <> 0 then
			mutex = LTrim(Left(line, pos - 1))
			rem WScript.StdOut.WriteLine(" Mutex found: " & mutex)
			command = "handle.exe -c " & mutex & " -y -p " & pid
			Set execClose = objShell.Exec(command)
			ok = False
			Do
			cline = execClose.StdOut.ReadLine()
			if cline = "Handle closed." then
				ok = True
			end if
			Loop While Not execClose.StdOut.atEndOfStream
			if not ok then
				MsgBox("Close handle failed")
			else
				count = count + 1
			end if
		end if
	end if
end if
Loop While Not objExec.StdOut.atEndOfStream

if count > 0 then
	MsgBox("Processed: " & count)
else
	MsgBox("Nothing processed")
end if
