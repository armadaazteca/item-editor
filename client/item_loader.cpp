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

#include "item_loader.hpp"

int ItemLoader::setProps(unsigned short attr, void* data, unsigned short size)
{
	//size
	if(!writeData(&size, sizeof(size), true))
		return getError();

	//attribute
	if(!writeData(&attr, sizeof(attr), true))
		return getError();

	//data
	if(!writeData(data, size, true))
		return getError();
	
	return ERROR_NONE;
}

int ItemLoader::setFlags(unsigned long flags)
{
	//data
	if(!writeData(&flags, sizeof(flags), true))
		return getError();

	return ERROR_NONE;
}

