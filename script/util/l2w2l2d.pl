#!/usr/bin/perl -w
#
# sample filter for convert script files from l2w to l2d
# require perl version >= v5.8.8
# by gyo
#
use strict;
use Encode;
use Encode::Guess;
use File::Basename;

my $l2wname = $ARGV[0];
my $l2dname = File::Basename::basename( $l2wname ) . ".txt";

open(IN,           "$l2wname") or die "error: can't read $l2wname: ";
open(OUT, '>:raw', "$l2dname") or die "error: can't write $l2dname: ";

while(<IN>){
	my $enc = guess_encoding($_, qw/euc-jp shiftjis 7bit-jis/);
	my $utf8;
	if(ref $enc){
		$utf8 = decode($enc->name, $_);
	} else {
		$utf8 = $_;
	}
	$utf8 =~ s/\x0D?\x0A?$//;
	$utf8 = conv($utf8) unless $utf8 =~ /^\/\//;
	#print OUT Encode::encode("UTF-8", "$utf8\n");
	print OUT Encode::encode('utf-16le', "$utf8\n");
}
close(OUT);
close(IN);


sub conv{
	local $_ = shift;
	#replacing keyword
	s/ +$//g;
	s/LABEL *\((.+)\)/$1:/ig;
	s/NPCSEL *\( *([^\[]+)\[ *ID=([0-9]+) *\] *\)/\/\/ $1\nSelectTarget($2)/ig;
	s/NPCSEL *\( *"?([^"]+[^" ])"? *\)/SelectTarget("$1")/ig;
	s/NPCDLG *\( *([^\[]+)\[ *ID=([0-9]+) *\] *\)/\/\/ $1\nNpcDialog($2)/ig;
	s/NPCDLG *\( *"?([^\[\]]+)"? *\)/\/\/ $1\nNpcDialog("$1")/ig;
	s/DLGSEL *\( *"?([^"]+[^" ])"? *\)/SelectDialog("$1")/ig;
	s/DELAY *\(/Delay(/ig;
	s/CALL *\( *"?(.+)"? *\)/Jump("$1")/ig;
	s/RETURN *\(*.*\)*/Return/ig;
	s/EXIT *\(*.*\)*/Exit/ig;
	s/MSG *\( *"?(.*)"? *\)/Msg(Console, "$1")/ig;
	s/USEITEM *\( *([^\[]+)\[ *ID=([0-9]+) *\] *\)/\/\/ $1\nUseItem($2)/ig;
	s/JMP *\( *"?(.+)"? *\)/Jump("$1")/ig;
	s/ITEMCOUNT *\( *([^\[]+)\[ *ID=([0-9]+) *\] *, *(.+) *, *(\-?[0-9]+) *\)/\/\/ $1\nIf (CountItem($2) $3 $4)/ig;
	s/SET *\( *fightstop *\)/BattleStop()/ig;
	s/SET *\( *fightstart *\)/BattleStop()/ig;
	s/SET *\(/\/\/SET\(/ig;
	s/GOHOME *\(.*\)/ReturnToVillage()/ig;
	s/MOVETO *\(/MoveTo(/ig;
	s/\( *(\-?[0-9]+) *, *(\-?[0-9]+) *, *(\-?[0-9]+) *\)/\($1, $2, $3\)/ig;
	s/POSINRANGE *\( *(\-?[0-9]+) *, *(\-?[0-9]+) *, *(\-?[0-9]+) *, *(\-?[0-9]+) *\)/If (LocInRange\($1, $2, $3, $4\)/ig;
	s/POSOUTRANGE *\( *(\-?[0-9]+) *, *(\-?[0-9]+) *, *(\-?[0-9]+) *, *(\-?[0-9]+) *\)/If (LocInRange\($1, $2, $3, $4\)\n{\n}\nElse/ig;
	s/CHARSTATUS *\( *CHP *, *(.+) *, *(\-?[0-9]+) */If (Char.HP $1 $2)/ig;
	s/CHARSTATUS *\( *HP *, *(.+) *, *(\-?[0-9]+) */If (Char.HP% $1 $2)/ig;
	s/BUYITEM *\( *([^\[]+)\[ *ID=([0-9]+) *\] *, *(\-?[0-9]+)[ ;]*\)/\/\/ $1\nAddItem($2, $3)/ig;
	s/\/\/$/\/\/ /g;

	return $_;
}
