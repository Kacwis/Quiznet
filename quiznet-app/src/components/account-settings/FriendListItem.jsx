import style from "./Settings.module.css";

import { getAvatarPathByNumber } from "../../constants/Constants";

import pt from "prop-types";
import FriendOptIcon from "./FriendOptIcon";
import { faTrashCan } from "@fortawesome/free-regular-svg-icons";
import { blockFriend, removeFriend } from "../../api";
import { faBan } from "@fortawesome/free-solid-svg-icons";

const FriendListItem = ({ friend, showResponseMsg }) => {
	const avatarPath = getAvatarPathByNumber(friend.avatarId);

	return (
		<li key={friend.id}>
			<div className={style["friend-list-item"]}>
				<div className={style["friend-info"]}>
					<img src={avatarPath} />
					<p>{friend.username}</p>
				</div>
				<div className={style["friend-item-opt"]}>
					<FriendOptIcon
						icon={faTrashCan}
						apiCall={removeFriend}
						friendId={friend.id}
						showResponseMsg={showResponseMsg}
						option={"remove"}
					/>
					<FriendOptIcon
						icon={faBan}
						apiCall={blockFriend}
						friendId={friend.id}
						showResponseMsg={showResponseMsg}
						option={"block"}
					/>
				</div>
			</div>
		</li>
	);
};

FriendListItem.propTypes = {
	friend: pt.object.isRequired,
	showResponseMsg: pt.func.isRequired,
};

export default FriendListItem;
