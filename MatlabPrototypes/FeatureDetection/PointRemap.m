function [px_out, py_out]=PointRemap(px, py, xLim, yLim, xPercent, yPercent)
	s_x= (xLim(2)-xLim(1))*xPercent;
	s_y= (yLim(2)-yLim(1))*yPercent;
	nl_x=xLim+[s_x, -s_x];
	nl_y=yLim+[s_y, -s_y];
	
	px_out=max(min(px, nl_x(2)), nl_x(1));
	py_out=max(min(py, nl_y(2)), nl_y(1));
end