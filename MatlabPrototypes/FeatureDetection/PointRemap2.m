function [p_ib, p_ob]=PointRemap2(p_in, xLim, yLim, xPercent, yPercent)
	s_x= (xLim(2)-xLim(1))*xPercent;
	s_y= (yLim(2)-yLim(1))*yPercent;
	nl_x=xLim+[s_x, -s_x];
	nl_y=yLim+[s_y, -s_y];
	
	p_ib=[NaN, NaN];
	p_ib(1)=max(min(p_in(1), nl_x(2)), nl_x(1));
	p_ib(2)=max(min(p_in(2), nl_y(2)), nl_y(1));
	
	p_ob=[NaN, NaN];	
	p_ob(1)=max(min(p_in(1), xLim(2)), xLim(1));
	p_ob(2)=max(min(p_in(2), yLim(2)), yLim(1));
		
end