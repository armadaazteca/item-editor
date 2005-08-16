//////////////////////////////////////////////////////////////////////
// OTItemEditor
//////////////////////////////////////////////////////////////////////
// 
//////////////////////////////////////////////////////////////////////
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software Foundation,
// Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//////////////////////////////////////////////////////////////////////

#ifndef __OTITEMEDITOR_GUI_H__
#define __OTITEMEDITOR_GUI_H__

#include "item_type.hpp"

class GUI{
protected:
	GUI(){};
public:
	virtual ~GUI(){}
	static GUI* getGUI();

	virtual void showSplash(){};
	virtual void hideSplash(){};

	virtual void initGUI() = 0;
	//return
	//true. can free dump
	//false. can not free dump
	virtual bool loadSpriteInternal(const unsigned char *, const unsigned long, InternalSprite* ){return true;};
	virtual void loadSpriteInternalTransparent(unsigned long color,  InternalSprite *sprite){};
	virtual void unloadSpriteInternal(InternalSprite){};
};


#endif